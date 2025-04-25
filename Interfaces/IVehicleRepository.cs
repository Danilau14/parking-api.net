using ParkingApi.Models;

namespace ParkingApi.Interfaces;

public interface IVehicleRepository : IBaseRepository<Vehicle>
{
    Task<Vehicle?> FindOneVehicleByLicencePlate(string licensePlate);

    Task<Vehicle> CreateVehicle(Vehicle vehicle);

    Task<Vehicle> UpdateVehicle(Vehicle vehicle);
}
