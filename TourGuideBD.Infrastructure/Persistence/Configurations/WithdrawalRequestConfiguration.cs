using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuideBD.Domain.Entities.Guide;

namespace TourGuideBD.Infrastructure.Persistence.Configurations;

public class WithdrawalRequestConfiguration : IEntityTypeConfiguration<WithdrawalRequest>
{
    public void Configure(EntityTypeBuilder<WithdrawalRequest> builder)
    {
        builder.ToTable("WithdrawalRequests");

        builder.Property(w => w.RequestedAmount).HasColumnType("decimal(10,2)");
        builder.Property(w => w.ProcessingFee).HasColumnType("decimal(10,2)");
        builder.Property(w => w.NetAmount).HasColumnType("decimal(10,2)");
        builder.Property(w => w.AdminNote).HasMaxLength(500);
        builder.Property(w => w.TransactionReference).HasMaxLength(200);
        builder.Property(w => w.ProcessedByUserId).HasMaxLength(450);

        builder.HasOne(w => w.GuideProfile)
            .WithMany()
            .HasForeignKey(w => w.GuideProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.PaymentMethod)
            .WithMany()
            .HasForeignKey(w => w.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(w => w.GuideProfileId);
        builder.HasIndex(w => w.Status);
    }
}