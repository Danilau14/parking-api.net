namespace ParkingApi.Repositories;

public class RevokedTokenRepository : BaseRepository<RevokedToken>, IRevokedTokenRepository
{
    public RevokedTokenRepository(ApplicationDbContext context) : base(context) { }


    public async Task<bool> IsTokenRevoked(string tokenToRevoke)
    {
        var token = await _dbSet.SingleOrDefaultAsync(revokedToken => revokedToken.Token == tokenToRevoke);

        if (token == null) return false;

        return true;
    }

    public async Task<bool> SaveTokenRevoked(RevokedToken revokedToken)
    {
        try
        {
            await _dbSet.AddRangeAsync(revokedToken);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
