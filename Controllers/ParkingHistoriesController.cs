namespace ParkingApi.Controllers;

/// <summary>
/// Controlador que gestiona el registro de  vehiculos en un parqueadero.
/// </summary>
/// <remarks>
/// Todos los endpoints de este controlador requieren autenticación mediante token JWT.
/// Incluye el siguiente header en cada solicitud:
/// <code>
/// Authorization: Bearer {token}
/// </code>
/// </remarks>
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

    /// <summary>
    /// Registra el ingreso de un vehículo a un parqueadero.
    /// </summary>
    /// <param name="CreateParkingHistoryDto">
    /// DTO que contiene la placa del vehiculo y el id del parqueadero.
    /// </param>
    /// <returns>
    /// Un código 201 que indica que el vehículo fue registrado exitosamente.
    /// </returns>
    /// <response code="201">Indica que el recurso fue creado correctamente.</response>
    /// <response code="400">Si no se logró crear el registro por errores de validación o disponibilidad.</response>
    /// <response code="401">Si el usuario no está autenticado.</response>
    /// <response code="403">Si el usuario no es un socio.</response>
    /// <exception cref="EipexException">
    /// Se lanza si ocurren errores al crear el registro, como falta de disponibilidad o conflicto de parqueo.
    /// </exception>
    /// <remarks>
    /// Este endpoint valida que el parqueadero tenga disponibilidad y que el vehículo no se encuentre ya registrado en otro parqueadero.
    /// </remarks>
    [HttpPost("check-in")]
    [Authorize(Policy = "PartnerOnly")]
    public async Task<IActionResult> CheckIn([FromBody] CreateParkingHistoryDto parkingHistoryDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await _parkingHistoryService.CreateParkingHistory(parkingHistoryDto, userId);

        return Created();
    }

    /// <summary>
    /// Registra la salida de un vehículo de un parqueadero.
    /// </summary>
    /// <param name="parkingHistoryDto">
    /// DTO que contiene la placa del vehículo y el ID del parqueadero.
    /// </param>
    /// <returns>
    /// Un objeto que contiene la información de ingreso, salida, tiempo y costo pagado por el vehículo.
    /// </returns>
    /// <response code="200">Indica que la salida del vehículo fue registrada exitosamente.</response>
    /// <response code="400">
    /// Si el vehículo, parqueadero, o el registro de ingreso no existen, o si el vehículo no se encuentra en el parqueadero.
    /// </response>
    /// <response code="401">Si el usuario no está autenticado.</response>
    /// <response code="403">Si el usuario no es un socio.</response>
    /// <exception cref="EipexException">
    /// Se lanza si ocurren errores al registrar la salida del vehículo.
    /// </exception>
    /// <remarks>
    /// Este endpoint valida que exista un registro previo del vehículo para actualizarlo con su fecha de salida.
    /// </remarks>
    [HttpPost("check-out")]
    [Authorize(Policy = "PartnerOnly")]
    public async Task<IActionResult> CheckOut([FromBody] CreateParkingHistoryDto parkingHistoryDto)
    {
        var parkingHistory = await _parkingHistoryService.CloseParkingHistory(parkingHistoryDto);

        var dto = _mapper.Map<ParkingHistoryDto>(parkingHistory);

        return Ok(dto);
    }

    /// <summary>
    /// Lista los vehículos que se encuentran estacionados en algún parqueadero.
    /// </summary>
    /// <param name="paginationDto">
    /// DTO que contiene la página y el límite de resultados.
    /// </param>
    /// <returns>
    /// Un objeto con la metadata de la paginación y un listado de vehículos, cada uno con su placa y el ID del parqueadero.
    /// </returns>
    /// <response code="200">Devuelve el listado de los vehículos estacionados.</response>
    /// <response code="401">Si el usuario no está autenticado.</response>
    /// <response code="403">Si el usuario no tiene permisos de administrador.</response>
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
