using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reservation.App.Domain.Entities;
using Reservation.App.Infrastructure.Identity.Models;

namespace Reservation.App.Infrastructure.Persistence;

public class ReservationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<User> ReservationUsers { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Agent> Agents { get; set; }

    // ...

    public ReservationDbContext(DbContextOptions<ReservationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReservationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
