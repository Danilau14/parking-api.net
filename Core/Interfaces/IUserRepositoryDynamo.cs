namespace ParkingApi.Core.Interfaces;

public interface IUserRepositoryDynamo : IBaseRepositoryDynamo<UserDynamo>
{
    Task<UserDynamo?> FindByEmail(string email);

    Task<(int, string)> CreateUser(UserDynamo user);
}
