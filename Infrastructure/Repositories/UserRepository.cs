namespace ParkingApi.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    public async Task<User?> FindByEmail(string email)
    {
        return await _dbSet.SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<(int, string)> CreateUser(User user)
    {
        var error = string.Empty;
        await _dbSet.AddAsync(user);            
        var isSaved =  await _context.SaveChangesAsync();

        if(isSaved<1)
        {
            error = "User Not Save";
        }

        return(isSaved, error);
    }

}
