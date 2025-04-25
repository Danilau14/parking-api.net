namespace ParkingApi.Dto.ParkingHistory;

public class ParkingHistoryDto
{
    public DateTime CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
    public float CostTotalParkingLot { get; set; }
    public int? TimeInParkingLot { get; set; }
    public int ParkingLotId { get; set; }
    public int VehicleId { get; set; }
}
