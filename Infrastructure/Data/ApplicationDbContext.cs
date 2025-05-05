using System.Diagnostics;
using Mono.TextTemplating;
using ParkingApi.Core.Enums;
using ParkingApi.Core.Interfaces;
using ParkingApi.Core.Models;

namespace ParkingApi.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IRabbitMQMessageBuilder _rabbitMQMessageBuilder;
    private readonly IUserContextService _userContextService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IRabbitMQMessageBuilder rabbitMQMessageBuilder,
        IUserContextService userContextService
        ): base(options) 
    {
        _rabbitMQMessageBuilder = rabbitMQMessageBuilder;
        _userContextService = userContextService;
    }

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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached)
            .ToList();

        var userId = _userContextService.GetCurrentUserId() ?? null;

        foreach (var entry in entries)
        {
            var action = GetActionFromState(entry.State);
            var response = GetActionResponse(action);

            try
            {
                int result = await base.SaveChangesAsync(cancellationToken);

                await _rabbitMQMessageBuilder.PublishAuditMessageAsync(
                    entity: entry.Entity.GetType().Name,
                    action: action,
                    state: true,
                    userId: userId,
                    response: response
                );

                return result;
            }
            catch (Exception ex)
            {
                await _rabbitMQMessageBuilder.PublishAuditMessageAsync(
                    entity: entry.Entity.GetType().Name,
                    action: action,
                    state: false,
                    userId: userId,
                    response: ex.Message
                );
                throw;
            }
        }

        return 0; 
    }

    private Actions GetActionFromState(EntityState state) => state switch
    {
        EntityState.Added => Actions.CREATE,
        EntityState.Modified => Actions.UPDATE,
        EntityState.Deleted => Actions.DELETE,
        _ => throw new ArgumentOutOfRangeException(nameof(state), $"Unsupported state: {state}")
    };

    private string GetActionResponse(Actions state) => state switch
    {
        Actions.CREATE => "Created resource",
        Actions.UPDATE => "Updated resource",
        Actions.DELETE => "Deleted resource",
        _ => throw new ArgumentOutOfRangeException(nameof(state), $"Unsupported state: {state}")
    };
}
