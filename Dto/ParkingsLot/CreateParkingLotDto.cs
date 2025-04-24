using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Dto.ParkingsLot;

public class CreateParkingLotDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The size must be greater than 0")]
    public required int Size {  get; set; }

    [Required]
    [Range(0.1, float.MaxValue, ErrorMessage = "The costPerHour must be greater than 0")]
    public required float CostPerHour { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Invalid PartnerId")]
    public int? PartnerId { get; set; }
}
