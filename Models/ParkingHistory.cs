using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Models;

public class ParkingHistory
{
    public int Id { get; set; }

    [Column(TypeName = "timestamptz")]
    public DateTime CheckInDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "timestamptz")]
    public DateTime? CheckOutDate { get; set; }

    public float CostTotalParkingLot { get; set; }

    public int? TimeInParkingLot { get; set; }

    [Required]
    public required int ParkingLotId { get; set; }

    [ForeignKey("ParkingLotId")]
    public required ParkingLot ParkingLot { get; set; }

    [Required]
    public required int VehicleId { get; set; }

    [ForeignKey("VehicleId")]
    public required Vehicle Vehicle { get; set; }
}
