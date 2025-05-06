namespace ParkingApi.Infrastructure.RepositoriesDynamo;

public class BaseRepositoryDynamo<T> : IBaseRepositoryDynamo<T> where T : class
{
    private readonly IDynamoDBContext _dynamoDbContext;

    public BaseRepositoryDynamo(IDynamoDBContext dynamoDbContext)
    {
        _dynamoDbContext = dynamoDbContext;
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await _dynamoDbContext.LoadAsync<T>(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var scanConditions = new List<ScanCondition>();
        var search = _dynamoDbContext.ScanAsync<T>(scanConditions);
        return await search.GetRemainingAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dynamoDbContext.SaveAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        await _dynamoDbContext.SaveAsync(entity);
    }

    public async Task DeleteAsync(string id)
    {
        await _dynamoDbContext.DeleteAsync<T>(id);
    }
}


