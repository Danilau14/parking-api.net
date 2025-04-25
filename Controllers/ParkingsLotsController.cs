using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ParkingApi.Models;
using ParkingApi.Dto.ParkingsLot;

namespace ParkingApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParkingsLotsController : ControllerBase
{
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IParkingLotService _parkingLotService;
    private readonly IMapper _mapper;

    public ParkingsLotsController(
        IParkingLotRepository parkingLotRepository, 
        IMapper mapper,
        IParkingLotService parkingLotService
        )
    {
        _parkingLotRepository = parkingLotRepository;
        _mapper = mapper;
        _parkingLotService = parkingLotService;
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CrearteParkinLot([FromBody] CreateParkingLotDto createParkingLotDto)
    {
        var parkingLot = _mapper.Map<ParkingLot>(createParkingLotDto);

        try
        {
            var parkingLotSaved = await _parkingLotService.CreateParkigLotAsync(parkingLot);

            var dto = _mapper.Map<ParkingLotDto>(parkingLotSaved);

            return Ok(dto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception)
        {
            return BadRequest("Error Saving Parking Lot");
        }
    }

    /*[HttpGet()]
    public async Task<IActionResult> FindParkingsLot([FromQuery] PaginationDto)
    {

    }*/

}
