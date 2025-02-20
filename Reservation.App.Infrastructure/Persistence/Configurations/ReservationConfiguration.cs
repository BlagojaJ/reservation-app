using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.App.Domain.Entities;

namespace Reservation.App.Infrastructure.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.Property(r => r.Password).HasMaxLength(6);
        builder.Property(r => r.Message).HasMaxLength(300);

        // builder
        //     .HasOne(r => r.Offer)
        //     .WithMany(o => o.Reservations)
        //     .HasForeignKey(r => r.OfferId);

        builder
            .HasOne(r => r.User)
            .WithMany(o => o.Reservations)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
