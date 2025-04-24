using ParkingApi.Data;

namespace ParkingApi.Interfaces;

public interface IUnitOfWork
{
    ApplicationDbContext Context { get; }

    IUserRepository UserRepository { get; }
}
