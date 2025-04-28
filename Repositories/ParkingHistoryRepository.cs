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

    public async Task<(List<ParkingHistory>, int)> FindVehiclesByParkingLot(
        int page = 1,
        int limit = 10,
        int? parkingLotId = null
    )
    {
        if (page < 1 || limit < 1)
        {
            throw new EipexException(new ErrorResponse
            {
                Message = "Page and limit must be greater than 0.",
                ErrorCode = "PAGE_LIMIT_INVALID"
            }, HttpStatusCode.BadRequest);
        }

        var query = _dbSet.Include(history => history.Vehicle)
                          .Include(history => history.ParkingLot)
                          .Where(history => history.CheckOutDate == null)
                          .Select(history => new
                          {
                              history.Id,
                              history.CheckInDate,
                              history.CheckOutDate,
                              history.Vehicle,
                              history.ParkingLot,
                              ParkingLotId = history.ParkingLot != null ? history.ParkingLot.Id : (int?)null,
                          });

        if (parkingLotId.HasValue)
        {
            query = query.Where(history => history.ParkingLotId == parkingLotId); 
        }

        var totalCount = await query.CountAsync();

        var parkingHistoryList = await query.Skip((page - 1) * limit)
                                            .Take(limit)
                                            .ToListAsync();

        var result = parkingHistoryList.Select(history => new ParkingHistory
        {
            Id = history.Id,
            CheckInDate = history.CheckInDate,
            CheckOutDate = history.CheckOutDate,
            Vehicle = history.Vehicle,
            VehicleId = history.Vehicle.Id,
            ParkingLot = history.ParkingLot,
            ParkingLotId = history.ParkingLot.Id,
        }).ToList();

        return (result, totalCount);
    }
}
