using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkingApi.Dto.User;
using ParkingApi.Interfaces;
using ParkingApi.Models;

namespace ParkingApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserServiceInterface _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserServiceInterface userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto data)
    {
        var user = _mapper.Map<User>(data);

        await _userService.CreateUserAsync(user);
      
        return Ok("Usuario creado");
    }
}