namespace ParkingApi.Core.Models;

[DynamoDBTable("Users")]
public class UserDynamo
{
    private string _email;

    [DynamoDBHashKey]
    [DynamoDBProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [DynamoDBProperty("recycleBin")]
    public bool RecycleBin { get; set; } = false;

    [DynamoDBProperty("password")]
    [Column(TypeName = "text")]
    public required string Password { get; set; }

    [DynamoDBProperty("email")]
    public required string Email
    {
        get => _email;
        set => _email = value.ToLower();
    }

    [DynamoDBProperty("role", typeof(EnumConverter<UserRole>))]
    public required UserRole Role { get; set; } = UserRole.PARTNER;
}
