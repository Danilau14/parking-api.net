using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ParkingApi.Models;

public class ParkingLot
{
    [Key]
    public int Id { get; private set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "The size must be greater than 0")]
    [DisallowNull]
    public required int Size { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "The size must be greater than 0")]
    [DisallowNull]
    public required int FreeSpaces { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "The size must be greater than 0")]
    public required float CostPerHour { get; set; }
    public bool RecycleBin { get; set; } = false;

    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public required User User { get; set; }
    
    public required ICollection<ParkingHistory> ParkingHistories { get; set; }

}
