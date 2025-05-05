namespace ParkingApi.Core.Models;

public class Vehicle
{
    private string _licensePlate = string.Empty;

    public int Id { get; private set; }

    public bool RecycleBin { get; set; } = false;

    public required bool IsParked { get; set; } = false;

    public required string LicensePlate
    {
        get => _licensePlate;
        set => _licensePlate = value.ToUpper();
    }

    public required List<ParkingHistory> ParkingHistories { get; set; }
}
