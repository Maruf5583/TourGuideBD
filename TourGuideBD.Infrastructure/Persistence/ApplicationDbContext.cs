using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Audit;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Entities.PlaceDetails;
using TourGuideBD.Domain.Entities.Realtime;
using TourGuideBD.Domain.Entities.Reviews;
using TourGuideBD.Domain.Entities.Trip;
using TourGuideBD.Domain.Entities.Users;
using TourGuideBD.Infrastructure.Persistence.Seed;

namespace TourGuideBD.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Division> Divisions => Set<Division>();
    public DbSet<District> Districts => Set<District>();
    public DbSet<Upazila> Upazilas => Set<Upazila>();
    public DbSet<Place> Places => Set<Place>();
    public DbSet<PlacePhoto> PlacePhotos => Set<PlacePhoto>();
    public DbSet<PlaceCategory> PlaceCategories => Set<PlaceCategory>();
    public DbSet<PlaceCategoryMap> PlaceCategoryMaps => Set<PlaceCategoryMap>();
    public DbSet<PlaceTag> PlaceTags => Set<PlaceTag>();

    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ReviewPhoto> ReviewPhotos => Set<ReviewPhoto>();
    public DbSet<ReviewReport> ReviewReports => Set<ReviewReport>();

    public DbSet<TransportType> TransportTypes => Set<TransportType>();
    public DbSet<TransportRate> TransportRates => Set<TransportRate>();
    public DbSet<Itinerary> Itineraries => Set<Itinerary>();
    public DbSet<ItineraryStop> ItineraryStops => Set<ItineraryStop>();

    public DbSet<FavouritePlace> FavouritePlaces => Set<FavouritePlace>();
    public DbSet<VisitHistory> VisitHistories => Set<VisitHistory>();
    public DbSet<CheckIn> CheckIns => Set<CheckIn>();
    public DbSet<SavedDistrict> SavedDistricts => Set<SavedDistrict>();

    public DbSet<LiveVisitor> LiveVisitors => Set<LiveVisitor>();
    public DbSet<WeatherAlert> WeatherAlerts => Set<WeatherAlert>();
    public DbSet<Broadcast> Broadcasts => Set<Broadcast>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public DbSet<BusStand> BusStands => Set<BusStand>();
    public DbSet<DistrictBusRoute> DistrictBusRoutes => Set<DistrictBusRoute>();

    public DbSet<GuideApplication> GuideApplications => Set<GuideApplication>();
    public DbSet<GuideProfile> GuideProfiles => Set<GuideProfile>();
    public DbSet<TourPackage> TourPackages => Set<TourPackage>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<GuideAvailability> GuideAvailabilities => Set<GuideAvailability>();
    public DbSet<GuideReview> GuideReviews => Set<GuideReview>();
    public DbSet<GuideLiveLocation> GuideLiveLocations => Set<GuideLiveLocation>();
    public DbSet<GuideRevenue> GuideRevenues => Set<GuideRevenue>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        LocationSeedData.Seed(builder);
        PlaceCategorySeedData.Seed(builder);
        TransportSeedData.Seed(builder);

        BusStandSeedData.Seed(builder);
        DistrictBusRouteSeedData.Seed(builder);
    }
}