namespace ParkingApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<ParkingLot> ParkingLots { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<ParkingHistory> ParkingHistories { get; set; }
    public DbSet<RevokedToken> RevokedTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<Vehicle>()
            .HasIndex(vehicle => vehicle.LicensePlate)
            .IsUnique();

        modelBuilder.Entity<RevokedToken>()
            .HasIndex(revokedToken => revokedToken.Token)
            .IsUnique();
    }
}
