namespace ParkingApi.Application.Features.ParkingLots.Dtos;

public class CreateParkingLotDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The size must be greater than 0")]
    public required int Size {  get; set; }

    [Required]
    [Range(0.1, float.MaxValue, ErrorMessage = "The costPerHour must be greater than 0")]
    public required float CostPerHour { get; set; }

    public string? PartnerId { get; set; }
}
