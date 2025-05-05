using ParkingApi.Core.Interfaces;
using ParkingApi.Infrastructure.Data;

namespace ParkingApi.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IUserRepository? _userRepository;
    private IRevokedTokenRepository? _revokedTokenRepository;
    private IParkingLotRepository? _parkingLotRepository;
    private IParkingHistoryRepository? _parkingHistoryRepository;   
    private IVehicleRepository? _vehicleRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    public ApplicationDbContext Context { get => _context; }

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
    public IRevokedTokenRepository RevokedTokenRepository => _revokedTokenRepository ??= new RevokedTokenRepository(_context);
    public IParkingLotRepository ParkingLotRepository => _parkingLotRepository ??= new ParkingLotRepository(_context);
    public IParkingHistoryRepository ParkingHistoryRepository => _parkingHistoryRepository ??= new ParkingHistoryRepository(_context);
    public IVehicleRepository VehicleRepository => _vehicleRepository ??= new VehicleRepository(_context);
}
