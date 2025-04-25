using Microsoft.EntityFrameworkCore;
using ParkingApi.Data;
using ParkingApi.Interfaces;
using ParkingApi.Models;

namespace ParkingApi.Repositories;

public class ParkingHistoryRepository : BaseRepository<ParkingHistory>, IParkingHistoryRepository
{
    public ParkingHistoryRepository(ApplicationDbContext context) : base(context) { }

    public async Task<ParkingHistory> CreateParkingHistory(ParkingHistory parkingHistory)
    {
        await _dbSet.AddAsync(parkingHistory);
        await _context.SaveChangesAsync();
        return parkingHistory;
    }

    public async Task<ParkingHistory?> FindOneParkingHistoryOpen(int vehicleId, int parkingLotId)
    {
        var parkingHistory = await _dbSet.FirstOrDefaultAsync(
                p => p.ParkingLotId == parkingLotId 
                && p.VehicleId == vehicleId
                && p.CheckOutDate == null
            );

        return parkingHistory;
    }
}
