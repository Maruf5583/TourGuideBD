
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Users;
using TourGuideBD.Infrastructure.ExternalServices;
using TourGuideBD.Infrastructure.Identity;
using TourGuideBD.Infrastructure.Persistence;
using TourGuideBD.Infrastructure.Realtime;

namespace TourGuideBD.Infrastructure;

public static class DependencyInjection
{
    public static async Task<IServiceCollection> AddInfrastructureServices(
       this IServiceCollection services, IConfiguration configuration)
    {
        // ---------- Database ----------
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        // ---------- Identity ----------
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // ---------- JWT ----------
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        // ---------- Authorization ----------
        services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))
            .AddPolicy("ModeratorOnly", policy => policy.RequireRole("Moderator", "Admin"))
            .AddPolicy("TourGuideOnly", policy => policy.RequireRole("TourGuide", "Admin"))
            .AddPolicy("RegisteredUser", policy =>
                policy.RequireRole("User", "Moderator", "Admin", "TourGuide"));

        // ---------- Redis (Optional) ----------
        // ---------- Redis ----------
        var redisConnectionString = configuration.GetConnectionString("Redis");
        var redisAvailable = false;

        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            try
            {
                // Actually connect করে test করো
                var redisConfig = StackExchange.Redis.ConfigurationOptions.Parse(
                    redisConnectionString);
                redisConfig.AbortOnConnectFail = false;
                redisConfig.ConnectTimeout = 3000;
                redisConfig.SyncTimeout = 3000;

                var multiplexer = StackExchange.Redis.ConnectionMultiplexer.Connect(redisConfig);

                // Ping করে confirm করো
                var db = multiplexer.GetDatabase();
                await db.PingAsync();

                services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(multiplexer);

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnectionString;
                    options.InstanceName = "TourGuideBD:";
                });

                services.AddScoped<ICacheService, RedisCacheService>();
                redisAvailable = true;

                Console.WriteLine("✅ Redis connected successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Redis unavailable: {ex.Message}. Using in-memory cache.");
                redisAvailable = false;
            }
        }

        if (!redisAvailable)
        {
            services.AddDistributedMemoryCache();
            services.AddScoped<ICacheService, InMemoryCacheService>();
        }
        // ---------- AWS S3 ----------
        services.AddScoped<IBlobStorageService, LocalStorageService>();
        // ---------- HttpContextAccessor ----------
        services.AddHttpContextAccessor();

        // ---------- Services ----------
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<INotificationService, SignalRNotificationService>();

        services.AddHttpClient<IMapboxService, MapboxService>();

        // ---------- Stripe ----------
        services.AddScoped<IStripeService, StripeService>();

        // ---------- SignalR ----------
        services.AddSignalR();

        return services;
    }
}