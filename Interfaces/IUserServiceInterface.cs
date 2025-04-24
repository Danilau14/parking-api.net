using System.Threading.Tasks;
using ParkingApi.Models;

namespace ParkingApi.Interfaces;

public interface IUserServiceInterface
{
    Task<User> CreateUserAsync(User user);
    Task<string?> LoginAsync(string email, string password);

}
