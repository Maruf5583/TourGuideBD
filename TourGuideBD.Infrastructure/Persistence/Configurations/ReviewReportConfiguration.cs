using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Reviews;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class ReviewReportConfiguration : IEntityTypeConfiguration<ReviewReport>
{
    public void Configure(EntityTypeBuilder<ReviewReport> builder)
    {
        builder.ToTable("ReviewReports");

        builder.Property(r => r.Reason).IsRequired().HasMaxLength(300);
        builder.Property(r => r.ResolutionNote).HasMaxLength(300);

        builder.HasOne(r => r.Review)
            .WithMany(rv => rv.Reports)
            .HasForeignKey(r => r.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.ReportedByUser)
            .WithMany()
            .HasForeignKey(r => r.ReportedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.Status);

        builder.Property(r => r.ResolvedByUserId).HasMaxLength(450);
    }
}