using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Guide;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class GuidePaymentMethodConfiguration : IEntityTypeConfiguration<GuidePaymentMethod>
{
    public void Configure(EntityTypeBuilder<GuidePaymentMethod> builder)
    {
        builder.ToTable("GuidePaymentMethods");

        builder.Property(g => g.MobileNumber).HasMaxLength(20);
        builder.Property(g => g.BankName).HasMaxLength(100);
        builder.Property(g => g.AccountName).HasMaxLength(100);
        builder.Property(g => g.AccountNumber).HasMaxLength(50);
        builder.Property(g => g.BranchName).HasMaxLength(100);
        builder.Property(g => g.RoutingNumber).HasMaxLength(20);

        builder.HasOne(g => g.GuideProfile)
            .WithMany()
            .HasForeignKey(g => g.GuideProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(g => g.GuideProfileId);
    }
}