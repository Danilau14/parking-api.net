using Microsoft.EntityFrameworkCore;
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

    public async Task<ParkingLot?> FindByParkingLotAndUser(int parkingLotId, int partnerId)
    {
        var parkingLot = await _dbSet.FirstOrDefaultAsync(
            p => p.Id == parkingLotId && p.UserId == partnerId
            );

        return parkingLot;
    }

    public async Task<ParkingLot> UpdatedParkingLot(ParkingLot parkingLot)
    {
        _dbSet.Update(parkingLot);
        await _context.SaveChangesAsync();
        return parkingLot;
    }
}
