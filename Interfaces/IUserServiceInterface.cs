namespace ParkingApi.Interfaces;

public interface IUserServiceInterface
{
    Task<User> CreateUserAsync(User user);
    Task<string?> LoginAsync(string email, string password);

}
