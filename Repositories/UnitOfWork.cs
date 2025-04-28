namespace ParkingApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IUserRepository? _userRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    public ApplicationDbContext Context { get => _context; }

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
}
