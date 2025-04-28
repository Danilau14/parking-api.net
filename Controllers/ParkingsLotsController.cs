namespace ParkingApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParkingsLotsController : ControllerBase
{
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IParkingLotService _parkingLotService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public ParkingsLotsController(
        IParkingLotRepository parkingLotRepository, 
        IMapper mapper,
        IParkingLotService parkingLotService,   
        IUserRepository userRepository
        )
    {
        _parkingLotRepository = parkingLotRepository;
        _mapper = mapper;
        _parkingLotService = parkingLotService;
        _userRepository = userRepository;
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
            return BadRequest(new { message = "Error Saving Parking Lot"});
        }
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> FindOneParkingLot(int id)
    {
        var parkingLot = await _parkingLotRepository.GetByIdAsync(id);

        if (parkingLot == null)
        {
            return NotFound(new { message = "Parking dont found" });
        }

        var dto = _mapper.Map<ParkingLotDto>(parkingLot);

        return Ok(dto);
    }

    [HttpPatch("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdatedParkingLot(int id, [FromBody] UpdatedParkingLotDto updatedParkingLotDto)
    {
        var parkingLot = await _parkingLotRepository.GetByIdAsync(id);

        if (parkingLot == null)
        {
            return NotFound(new { message = "Parking dont found" });
        }

        if(updatedParkingLotDto.PartnerId.HasValue)
        {
            var partner = await _userRepository.GetByIdAsync(updatedParkingLotDto.PartnerId.Value);

            if (partner == null)
            {
                return NotFound(new { message = "Invalid PartnerId" });
            }

            parkingLot.User = partner;
            parkingLot.UserId = partner.Id;
        }

        if (updatedParkingLotDto.Size.HasValue)
        {
            if (updatedParkingLotDto.Size.Value > parkingLot.Size)
            {
                parkingLot.FreeSpaces = parkingLot.FreeSpaces + (updatedParkingLotDto.Size.Value - parkingLot.Size);
                parkingLot.Size = updatedParkingLotDto.Size.Value;
            }

            if (updatedParkingLotDto.Size.Value < parkingLot.Size)
            {
                if (parkingLot.FreeSpaces == 0 || parkingLot.Size - parkingLot.FreeSpaces > updatedParkingLotDto.Size.Value)
                {
                    return BadRequest(new { message = "The size of the parking lot cannot be less than the number of current vehicles." });
                }

                parkingLot.FreeSpaces = parkingLot.FreeSpaces - (parkingLot.Size - updatedParkingLotDto.Size.Value);
                parkingLot.Size = updatedParkingLotDto.Size.Value;
            }
        }

        if (updatedParkingLotDto.CostPerHour.HasValue)
        {
            parkingLot.CostPerHour = updatedParkingLotDto.CostPerHour.Value;
        }

        var parkingLotUpdate = await _parkingLotRepository.UpdatedParkingLot(parkingLot);

        var dto = _mapper.Map<ParkingLotDto>(parkingLotUpdate);

        return Ok(dto);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> RemoveParkingLot(int id)
    {
        var parkingLot = await _parkingLotRepository.GetByIdAsync(id);

        if (parkingLot == null)
        {
            return NotFound(new { message = "Parking dont found" });
        }

        parkingLot.RecycleBin = true;

        var parkingLotUpdate = await _parkingLotRepository.UpdatedParkingLot(parkingLot);

        var dto = _mapper.Map<ParkingLotDto>(parkingLotUpdate);

        return Ok(dto);
    }

    [HttpGet("all")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> FindParkingsLot([FromQuery] PaginationDto paginationDto)
    {
        var (parkingsLot, totalCount) = await _parkingLotRepository.FindAndCountAsync(
                paginationDto.Page,
                paginationDto.Limit
            );

        var parkingsLotDto = _mapper.Map<List<ParkingLotDto>>(parkingsLot);

        var result = new
        {
            Total = totalCount,
            paginationDto.Page,
            TotalPage = (int)Math.Ceiling((double)totalCount / paginationDto.Limit),
            Data = parkingsLotDto
        };

        return Ok(result);
    }
}
