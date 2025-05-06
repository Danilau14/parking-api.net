namespace ParkingApi.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IDynamoDBContext _dynamoDBContext;
    private IUserRepository? _userRepository;
    private IUserRepositoryDynamo? _userRepositoryDynamo;
    private IRevokedTokenRepository? _revokedTokenRepository;
    private IParkingLotRepository? _parkingLotRepository;
    private IParkingHistoryRepository? _parkingHistoryRepository;   
    private IVehicleRepository? _vehicleRepository;

    public UnitOfWork(ApplicationDbContext context, IDynamoDBContext dynamoDBContext)
    {
        _context = context;
        _dynamoDBContext = dynamoDBContext;
    }
    public ApplicationDbContext Context { get => _context; }

    public IUserRepositoryDynamo UserRepositoryDynamo => _userRepositoryDynamo ??= new UserRepositoryDynamo(_dynamoDBContext);

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
    public IRevokedTokenRepository RevokedTokenRepository => _revokedTokenRepository ??= new RevokedTokenRepository(_context);
    public IParkingLotRepository ParkingLotRepository => _parkingLotRepository ??= new ParkingLotRepository(_context);
    public IParkingHistoryRepository ParkingHistoryRepository => _parkingHistoryRepository ??= new ParkingHistoryRepository(_context);
    public IVehicleRepository VehicleRepository => _vehicleRepository ??= new VehicleRepository(_context);
}
