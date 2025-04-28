namespace ParkingApi.Dto.ParkingsLot;

public class UpdatedParkingLotDto 
{
    [Range(1, int.MaxValue, ErrorMessage = "The size must be greater than 0")]
    public int? Size { get; set; }

    [Range(0.1, float.MaxValue, ErrorMessage = "The costPerHour must be greater than 0")]
    public float? CostPerHour { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid PartnerId")]
    public int? PartnerId { get; set; }
}
