namespace ParkingApi.Core.Models;

[DynamoDBTable("Users")]
public class UserDynamo
{
    private string _email;

    [DynamoDBHashKey]
    public int Id { get; set; }

    [DynamoDBProperty]
    public bool RecycleBin { get; set; } = false;

    [DynamoDBProperty]
    [Column(TypeName = "text")]
    public required string Password { get; set; }

    [DynamoDBProperty]
    public required string Email
    {
        get => _email;
        set => _email = value.ToLower();
    }

    [DynamoDBProperty(typeof(EnumConverter<UserRole>))]
    public required UserRole Role { get; set; } = UserRole.PARTNER;
}
