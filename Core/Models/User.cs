using ParkingApi.Core.Enums;

namespace ParkingApi.Core.Models;

public class User
{
    private string _email;
    private List<ParkingLot> _parkingLots;

    public int Id { get; set; }

    public bool RecycleBin { get; set; } = false;

    [Required]
    [DisallowNull]
    [Column(TypeName = "text")]
    public required string Password { get; set; }

    [Required]
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
