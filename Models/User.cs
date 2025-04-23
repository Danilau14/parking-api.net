using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using ParkingApi.Models.Enums;

namespace ParkingApi.Models;

[Table("User")]
public class User : IdentityUser
{
    public bool RecycleBin { get; set; } = false;

    [Required]
    [DisallowNull]
    [Column(TypeName = "text")]
    public required string Password { get; set; }
    public required List<ParkingLot> ParkingLots { get; set; }
}
