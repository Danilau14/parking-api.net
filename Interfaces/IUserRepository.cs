namespace ParkingApi.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByEmail(string email);

    Task<(int, string)> CreateUser(User user);
}
