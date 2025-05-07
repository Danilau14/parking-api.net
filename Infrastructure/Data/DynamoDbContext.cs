using Amazon.Runtime;

namespace ParkingApi.Infrastructure.Data;

public static class DynamoDbContext
{
    public static void AddDynamoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var dynamoDbSettings = configuration.GetSection("DynamoDbSettings").Get<DynamoDbSettings>();

        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = "http://localhost:4566",
            UseHttp = true,
        };

        var credentials = new BasicAWSCredentials("DUMMYIDEXAMPLE", "DUMMYIDEXAMPLE");

        //Console.WriteLine(config.ServiceURL);
        var client = new AmazonDynamoDBClient(credentials, config);

        services.AddSingleton<IAmazonDynamoDB>(client);
        services.AddSingleton<IDynamoDBContext>(sp => new DynamoDBContext(client));
    }
}
