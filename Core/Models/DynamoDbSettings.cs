namespace ParkingApi.Core.Models;

public class DynamoDbSettings
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string Region { get; set; }
    public string ApiKey { get; set; }
    public string SecretKey { get; set; }
}
