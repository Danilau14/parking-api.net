namespace ParkingApi.Repositories;

public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Vehicle?> FindOneVehicleByLicencePlate(string licensePlate)
    {
        var vehicle  =  await _dbSet.FirstOrDefaultAsync(vehicle => vehicle.LicensePlate ==  licensePlate);

        return vehicle;
    }

    public async Task<Vehicle> CreateVehicle(Vehicle vehicle)
    {
        await _dbSet.AddAsync(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }

    public async Task<Vehicle> UpdateVehicle(Vehicle vehicle)
    {
        _dbSet.Update(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }
}
