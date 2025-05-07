namespace ParkingApi.Core.Models;

[DynamoDBTable("ParkingsLot")]

public class ParkingLotDynamo
{
    [DynamoDBHashKey]
    [DynamoDBProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [DynamoDBProperty("size")]
    public int Size { get; set; }

    [DynamoDBProperty("freeSpaces")]
    public int FreeSpaces { get; set; }

    [DynamoDBProperty("costPerHour")]
    public float CostPerHour { get; set; }

    [DynamoDBProperty("recycleBin")]
    public bool RecycleBin { get; set; } = false;

    [DynamoDBProperty("userId")]
    public string UserId { get; set; } = default!;
}
