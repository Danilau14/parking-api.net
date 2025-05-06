namespace ParkingApi.Infrastructure.Data;

public static class DynamoDbContext
{
    public static void AddDynamoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var dynamoDbSettings = configuration.GetSection("DynamoDbSettings").Get<DynamoDbSettings>();


        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = $"http://{dynamoDbSettings.Host}:{dynamoDbSettings.Port}", 
            RegionEndpoint = RegionEndpoint.GetBySystemName(dynamoDbSettings.Region) 
        };

        Console.WriteLine(config.ServiceURL);

        var client = new AmazonDynamoDBClient(dynamoDbSettings.ApiKey, dynamoDbSettings.SecretKey, config);

        services.AddSingleton<IAmazonDynamoDB>(client);
        services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
    }
}
