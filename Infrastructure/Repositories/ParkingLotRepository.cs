using ParkingApi.Core.Interfaces;
using ParkingApi.Infrastructure.Data;

namespace ParkingApi.Infrastructure.Repositories;

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
                p => p.Id == parkingLotId 
                && p.UserId == partnerId
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
            throw new EipexException(new ErrorResponse
            {
                Message = "Page and limit must be greater than 0.",
                ErrorCode = ErrorsCodeConstants.PAGE_LIMIT_INVALID
            }, HttpStatusCode.BadRequest);
        }

        var query = _dbSet.Include(pl => pl.User)
                            .AsNoTracking()
                            .Select(pl => new
                            {
                                pl.Id,
                                pl.Size,
                                pl.CostPerHour,
                                pl.UserId,
                                pl.FreeSpaces,
                                pl.RecycleBin,
                            })
                            .Where(pl => pl.RecycleBin == false);

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
