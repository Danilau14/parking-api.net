using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ParkingApi.Interfaces;
using ParkingApi.Models;
using ParkingApi.Dto.User;

namespace ParkingApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserServiceInterface _userService;
    private readonly IRevokedTokenRepository _revokedTokenRepository;

    public AuthController(
        IUserServiceInterface userService,
        IRevokedTokenRepository revokedTokenRepository
        )
    {
        _userService = userService;
        _revokedTokenRepository = revokedTokenRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _userService.LoginAsync(loginDto.Email, loginDto.Password);

        if (token == null)
        {
            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        return Ok(new { token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var bearerToken  =  Request.Headers.Authorization.ToString();
        if (null == bearerToken) return BadRequest("No token provided");

        var jwt = bearerToken.Split(' ')[1];

        bool result = await _revokedTokenRepository.SaveTokenRevoked(
            new RevokedToken { Token = jwt }
            );

        if (!result) return BadRequest("The token could not be revoked.");

        return Ok("Logged out successfully");
     }

    [Authorize]
    [HttpGet("me")]
    [Authorize(Policy = "PartnerOnly")]
    public IActionResult GetCurrentUserInfo()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            Email = email,
            Role = role
        });
    }
}
