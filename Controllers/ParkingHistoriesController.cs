using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingApi.Dto.ParkingHistory;
using ParkingApi.Interfaces;

namespace ParkingApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParkingHistoriesController : ControllerBase
{
    private readonly IParkingHistoryService _parkingHistoryService;

    public ParkingHistoriesController(IParkingHistoryService parkingHistoryService)
    {
        _parkingHistoryService = parkingHistoryService;
    }

    [HttpPost("check-in")]
    [Authorize(Policy = "PartnerOnly")]
    public async Task<IActionResult> CheckIn([FromBody] CreateParkingHistoryDto parkingHistoryDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var parkingHistory = await _parkingHistoryService.CreateParkingHistory(parkingHistoryDto, userId);

        return Created();
    }
}
