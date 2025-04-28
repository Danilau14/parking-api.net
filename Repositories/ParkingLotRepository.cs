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
        var parkingLot = await _dbSet.Include(p => p.User).FirstOrDefaultAsync(
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

    public async Task<(List<ParkingLot>, int)> FindAndCountAsync(
        int page = 1, 
        int limit = 10, 
        int? userId = null
       )
    {
        if (page < 1 || limit < 1)
        {
            throw new ArgumentException("Page and limit must be greater than 0.");
        }

        var query = _dbSet.Include(pl => pl.User) 
                            .Select(pl => new
                            {
                                pl.Id,
                                pl.Size,
                                pl.CostPerHour,
                                pl.UserId,
                                pl.FreeSpaces
                            });

        if (userId.HasValue)
        {
            query = query.Where(pl => pl.UserId == userId);
        }

        var totalCount = await query.CountAsync();

        var parkingLots = await query.Skip((page - 1) * limit)
                                     .Take(limit)
                                     .ToListAsync();

        return (
            parkingLots.Select(pl => new ParkingLot
            {
                Id = pl.Id,
                Size = pl.Size,
                CostPerHour = pl.CostPerHour,
                UserId = pl.UserId,
                FreeSpaces = pl.FreeSpaces,
                ParkingHistories = [],
            }).ToList(), totalCount);
    }
}
