
namespace ParkingApi.Dto.Email;

public class EmailDto
{
    public string Email { get; set; } = null!;
    public string LicensePlate { get; set; } = null!;
    public string Message { get; set; } = null!;
    public int ParkingLotId { get; set; }
}
