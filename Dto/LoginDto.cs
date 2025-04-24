using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Dto;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}
