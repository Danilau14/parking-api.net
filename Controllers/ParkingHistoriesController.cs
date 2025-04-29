namespace ParkingApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParkingHistoriesController : ControllerBase
{
    private readonly IParkingHistoryService _parkingHistoryService;
    private readonly IParkingHistoryRepository _parkingHistoryRepository;
    private readonly IMapper _mapper;

    public ParkingHistoriesController(
        IParkingHistoryService parkingHistoryService, 
        IMapper mapper,
        IParkingHistoryRepository parkingHistoryRepository
        )
    {
        _parkingHistoryService = parkingHistoryService;
        _mapper = mapper;
        _parkingHistoryRepository = parkingHistoryRepository;
    }

    [HttpPost("check-in")]
    [Authorize(Policy = "PartnerOnly")]
    public async Task<IActionResult> CheckIn([FromBody] CreateParkingHistoryDto parkingHistoryDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _parkingHistoryService.CreateParkingHistory(parkingHistoryDto, userId);

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

    [HttpGet("all-vehicles-parked")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> FindParkingsLot([FromQuery] PaginationDto paginationDto)
    {
        var (parkingHistories, totalCount) = await _parkingHistoryRepository.FindVehiclesByParkingLot(
                paginationDto.Page,
                paginationDto.Limit
            );

        var parkingsLotDto = _mapper.Map<List<ParkingHistoryDto>>(parkingHistories);

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
