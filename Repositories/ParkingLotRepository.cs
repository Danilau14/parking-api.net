using ParkingApi.Data;
using ParkingApi.Interfaces;
using ParkingApi.Models;

namespace ParkingApi.Repositories;

public class ParkingLotRepository : BaseRepository<ParkingLot>, IParkingLotRepository
{
    public ParkingLotRepository(ApplicationDbContext context) : base(context) { }

    public async Task<ParkingLot> CreateParkingLot(ParkingLot parkingLot)
    {
        await _dbSet.AddAsync(parkingLot);
        await _context.SaveChangesAsync();
        return parkingLot;
    }
}
