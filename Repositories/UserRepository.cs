using Microsoft.EntityFrameworkCore;
using ParkingApi.Data;
using ParkingApi.Interfaces;
using ParkingApi.Models;

namespace ParkingApi.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    public async Task<User?> FindByEmail(string email)
    {
        return await _dbSet.SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> CreateUser(User user)
    {
        await _dbSet.AddAsync(user);            
        await _context.SaveChangesAsync();    
        return user;
    }

}
