using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Dto.ParkingHistory;

public class CreateParkingHistoryDto
{
    public required string _licencePlate;

    [Required]
    [Length(6,6)]
    [RegularExpression(@"^[A-Z]{3}[0-9]{3}$", ErrorMessage = "Invalid format in license plate")]
    public required string LicensePlate {
        get => _licencePlate;
        set => _licencePlate = value.ToUpper().Trim();
    }

    [Required]
    [Range(1,int.MaxValue, ErrorMessage ="ParkingLot Invalid") ]
    public required int ParkingLotId { get; set; }
}
