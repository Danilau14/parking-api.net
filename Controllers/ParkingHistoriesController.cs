using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingApi.Dto.ParkingHistory;
using ParkingApi.Dto.ParkingsLot;
using ParkingApi.Interfaces;

namespace ParkingApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParkingHistoriesController : ControllerBase
{
    private readonly IParkingHistoryService _parkingHistoryService;
    private readonly IMapper _mapper;

    public ParkingHistoriesController(IParkingHistoryService parkingHistoryService, IMapper mapper)
    {
        _parkingHistoryService = parkingHistoryService;
        _mapper = mapper;
    }

    [HttpPost("check-in")]
    [Authorize(Policy = "PartnerOnly")]
    public async Task<IActionResult> CheckIn([FromBody] CreateParkingHistoryDto parkingHistoryDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var parkingHistory = await _parkingHistoryService.CreateParkingHistory(parkingHistoryDto, userId);

        return Created();
    }

    [HttpPost("check-out")]
    [Authorize(Policy = "PartnerOnly")]
    public async Task<IActionResult> CheckOut([FromBody] CreateParkingHistoryDto parkingHistoryDto)
    {
        var parkingHistory = await _parkingHistoryService.CloseParkingHistory(parkingHistoryDto);

        var dto = _mapper.Map<ParkingHistoryDto>(parkingHistory);

        return Ok(dto);
    }

}
