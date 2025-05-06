namespace ParkingApi.Infrastructure.RepositoriesDynamo;

public class UserRepositoryDynamo : BaseRepositoryDynamo<UserDynamo>, IUserRepositoryDynamo
{
    private readonly IDynamoDBContext _context;

    public UserRepositoryDynamo(IDynamoDBContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserDynamo?> FindByEmail(string email)
    {
        var conditions = new List<ScanCondition>
        {
            new ScanCondition("Email", ScanOperator.Equal, email.ToLower())
        };

        var search = _context.ScanAsync<UserDynamo>(conditions);
        var result = await search.GetNextSetAsync();
        return result.FirstOrDefault();
    }

    public async Task<(int, string)> CreateUser(UserDynamo user)
    {
        try
        {
            await _context.SaveAsync(user);
            return (1, string.Empty);
        }
        catch (Exception ex)
        {
            return (0, ex.Message);
        }
    }

    public Task<UserDynamo?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(UserDynamo entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(UserDynamo entity)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}
