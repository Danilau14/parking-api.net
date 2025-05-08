
using ParkingApi.Core.Models;

namespace ParkingApi.Infrastructure.RepositoriesDynamo;

public class ParkingLotRepositoryDynamo : BaseRepositoryDynamo<ParkingLotDynamo>, IParkingLotRepositoryDynamo
{
    private readonly IDynamoDBContext _context;

    public ParkingLotRepositoryDynamo(IDynamoDBContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ParkingLotDynamo> CreateParkingLot(ParkingLotDynamo parkingLot)
    {
        await _context.SaveAsync(parkingLot);
        return parkingLot;
    }

    public Task<(List<ParkingLotDynamo>, int)> FindAndCountAsync(int page, int limit, int? userId = null)
    {
        throw new NotImplementedException();
    }

    public Task<ParkingLotDynamo?> FindByParkingLotAndUser(int parkingLotId, int partnerId)
    {
        throw new NotImplementedException();
    }

    public async Task<ParkingLotDynamo> UpdatedParkingLot(ParkingLotDynamo parkingLot)
    {
        await _context.SaveAsync(parkingLot);
        return parkingLot;
    }
}
