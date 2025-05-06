namespace ParkingApi.Application.Features.ParkingLots.Dtos;

public class ParkingLotDto
{
    public required int Id{ get; set; }

    public required int Size { get; set; }

    public required float CostPerHour { get; set; }

    public required float FreeSpaces { get; set; }

    public int? PartnerId { get; set; }

    public bool RecycleBin { get; set; }
}
