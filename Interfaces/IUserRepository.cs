namespace ParkingApi.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByEmail(string email);

    Task<User> CreateUser(User user);
}
