namespace ParkingApi.Models;

public class User
{
    public required string _email;
    public required List<ParkingLot> _parkingLots;

    [Key]
    public required int Id { get; set; }

    public bool RecycleBin { get; set; } = false;

    [Required]
    [DisallowNull]
    [Column(TypeName = "text")]
    public required string Password { get; set; }

    [EmailAddress]
    public required string Email
    {
        get => _email;
        set => _email = value.ToLower();
    }

    [Required]
    public required UserRole Role { get; set; } = UserRole.PARTNER;
    public required List<ParkingLot> ParkingLots { get => _parkingLots; set => _parkingLots = value; }
}
