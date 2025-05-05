namespace ParkingApi.Core.Models;

public class RevokedToken
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Token { get; set; }
}
