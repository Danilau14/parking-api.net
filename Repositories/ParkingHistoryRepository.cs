using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ParkingApi.Data;
using ParkingApi.Interfaces;
using ParkingApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public async Task<ParkingHistory> UpdateParkingHistory(ParkingHistory parkingHistory)
    {
        _dbSet.Update(parkingHistory);
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

    public async Task<ParkingHistory> UpdateTimeAndCostInParkingHistory(
            float costPerHour,
            ParkingHistory parkingHistory
        )
    {
        if(null != parkingHistory.CheckOutDate)
        {
            var duration = parkingHistory.CheckOutDate.Value - parkingHistory.CheckInDate;
            var seconds = (int)duration.TotalSeconds;
            parkingHistory.TimeInParkingLot = seconds;
            var cost = (costPerHour * seconds) / 3600;
            parkingHistory.CostTotalParkingLot = (float)Math.Round(cost, 2);
            return await this.UpdateParkingHistory(parkingHistory);
            
        }

        throw new BadHttpRequestException("Dates must be different null");
    }
}
