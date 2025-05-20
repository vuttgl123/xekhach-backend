using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LuanAnTotNghiep_TuanVu_TuBac.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=ConnectionStrings:DefaultConnection");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable(tb => tb.HasTrigger("trg_Users_Update"));

            modelBuilder.Entity<Ride>()
                .HasOne(r => r.RouteTripSchedule)
                .WithMany()
                .HasForeignKey(r => r.RouteTripScheduleId);

            modelBuilder.Entity<RouteTripSchedule>()
                .HasOne(rts => rts.VehicleDriver)
                .WithMany(vd => vd.RouteTripSchedules)
                .HasForeignKey(rts => rts.VehicleDriverId);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<RouteTrip> RouteTrips { get; set; }
        public DbSet<RouteTripSchedule> RouteTripSchedules { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Driver> Drivers { get; set; }          
        public DbSet<VehicleDriver> VehicleDrivers { get; set; }

        public DbSet<Promotion> Promotions { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Admin> Admins { get; set; }
    }

}
