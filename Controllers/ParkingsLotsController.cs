using ParkingApi.Application.Features.ParkingLots.Dtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ParkingApi.Controllers;

/// <summary>
/// Controlador que gestiona los parqueaderos.
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
public class ParkingsLotsController : ControllerBase
{
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IParkingLotService _parkingLotService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ParkingsLotsController(
        IParkingLotRepository parkingLotRepository, 
        IMapper mapper,
        IParkingLotService parkingLotService,   
        IUserRepository userRepository,
        IMediator mediator  

    )
    {
        _parkingLotRepository = parkingLotRepository;
        _mapper = mapper;
        _parkingLotService = parkingLotService;
        _userRepository = userRepository;
        _mediator = mediator;
    }

    /// <summary>
    /// Crea un parqueadero con o sin socio.
    /// </summary>
    /// <param name="createParkingLotDto">
    /// DTO contiene la capacidad del parqueadero, el costo por hora y el socio
    /// </param>
    /// <returns>
    /// Un objeto con las propidades del parqueadeo que acaba de crearse
    /// </returns>
    /// <response code="200">Devuelve el objeto con las propiedades del paqueadero.</response>
    /// <response code="401">Si el usuario no está autenticado.</response>
    /// <response code="403">Si el usuario no tiene permisos de administrador.</response>
    /// <response code="404">Si es asociado un partnerId que no existe.</response>
    /// <exception cref="EipexException">
    /// Se lanza si ocurren errores al crear un parqueadero.
    /// </exception>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CrearteParkingLot([FromBody] CreateParkingLotDto createParkingLotDto)
    {
        var command = new CreateParkingLotsCommand(
                createParkingLotDto.Size,
                createParkingLotDto.CostPerHour,
                createParkingLotDto.PartnerId
            );

        var parkingLotSaved = await _mediator.Send(command);
        var dto = _mapper.Map<ParkingLotDto>(parkingLotSaved);
        return Ok(dto);
    }

    /// <summary>
    /// Obtiene un parqueadero por Id.
    /// </summary>
    /// <param name="id">
    /// Es el ID del parqueadero
    /// </param>
    /// <returns>
    /// Un objeto con las propidades del parqueadero asociado al ID
    /// </returns>
    /// <response code="200">Devuelve un objeto con las propiedaes del parquedero.</response>
    /// <response code="401">Si el usuario no está autenticado.</response>
    /// <response code="403">Si el usuario no tiene permisos de administrador.</response>
    /// <response code="404">No se encuentra un parqueadero asociado al ID.</response>
    /// <exception cref="EipexException">
    /// Se lanza si ocurren errores al buscar el parqueadero.
    /// </exception>
    [HttpGet("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> FindOneParkingLot(int id)
    {
        var command = new FindOneByIdParkingLotCommand(id);
        var parkingLotSaved = await _mediator.Send(command);
        var dto = _mapper.Map<ParkingLotDto>(parkingLotSaved);
        return Ok(dto);
    }

    /// <summary>
    /// Actualiza un parqueadero existente.
    /// </summary>
    /// <param name="id">
    /// Identificador único del parqueadero a actualizar. Se obtiene desde la ruta.
    /// </param>
    /// <param name="updatedParkingLotDto">
    /// DTO que contiene los datos actualizables del parqueadero: capacidad, costo por hora y el ID del socio.
    /// </param>
    /// <returns>
    /// Un objeto con las propiedades actualizadas del parqueadero.
    /// </returns>
    /// <response code="200">Devuelve el parqueadero actualizado.</response>
    /// <response code="400">Si el DTO tiene datos inválidos</response>
    /// <response code="401">Si el usuario no está autenticado.</response>
    /// <response code="403">Si el usuario no tiene permisos de administrador.</response>
    /// <response code="404">Si no existe un parqueadero con el ID proporcionado.</response>
    /// <response code="409">No puede asociar una menor cantidad de espacios a los ocupados.</response>
    /// <exception cref="EipexException">
    /// Se lanza si ocurren errores al actualizar el parqueadero, como valores inválidos o entidades no encontradas.
    /// </exception>
    [HttpPatch("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdatedParkingLot( int id, [FromBody] UpdatedParkingLotDto updatedParkingLotDto)
    {
        var parkingLot = await _parkingLotRepository.GetByIdAsync(id);

        if (parkingLot == null)
        {
            throw new EipexException(new ErrorResponse
                {
                    Message = "Parking dont found",
                    ErrorCode = ErrorsCodeConstants.PARKINGLOT_NOT_FOUND
                }, HttpStatusCode.NotFound
            );
        }

        if (updatedParkingLotDto.PartnerId.HasValue)
        {
            var partner = await _userRepository.GetByIdAsync(updatedParkingLotDto.PartnerId.Value);

            if (partner == null)
            {
                throw new EipexException(new ErrorResponse
                    {
                        Message = "Invalid PartnerId",
                        ErrorCode = ErrorsCodeConstants.PARKINGLOT_INVALID
                    }, HttpStatusCode.NotFound
                );
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
                    throw new EipexException(new ErrorResponse
                        {
                            Message = "The size of the parking lot cannot be less than the number of current vehicles.",
                            ErrorCode = ErrorsCodeConstants.PARKINGLOT_CONFLICT
                        }, HttpStatusCode.Conflict
                    );
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

    /// <summary>
    /// Remueve logicamente un paqueadero.
    /// </summary>
    /// <param name="id">
    /// Identificador único del parqueadero a removerse. Se obtiene desde la ruta.
    /// </param>
    /// <returns>
    /// Un objeto con las propiedades con la propiedad recycleBin en true.
    /// </returns>
    /// <response code="200">Devuelve el parquedero con la propiedad recycleBin en true..</response>
    /// <response code="400">Si el DTO tiene datos inválidos</response>
    /// <response code="401">Si el usuario no está autenticado.</response>
    /// <response code="403">Si el usuario no tiene permisos de administrador.</response>
    /// <response code="404">Si no existe un parqueadero con el ID proporcionado.</response>
    /// <exception cref="EipexException">
    /// Se lanza si ocurren errores al actualizar el parqueadero, como valores inválidos o entidades no encontradas.
    /// </exception>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> RemoveParkingLot(int id)
    {
        var parkingLot = await _parkingLotRepository.GetByIdAsync(id);

        if (parkingLot == null)
        {
            throw new EipexException(new ErrorResponse
                {
                    Message = "Parking dont found",
                    ErrorCode = ErrorsCodeConstants.PARKINGLOT_NOT_FOUND
                }, HttpStatusCode.NotFound
            );        
        }

        parkingLot.RecycleBin = true;

        var parkingLotUpdate = await _parkingLotRepository.UpdatedParkingLot(parkingLot);

        var dto = _mapper.Map<ParkingLotDto>(parkingLotUpdate);

        return Ok(dto);
    }

    /// <summary>
    /// Lista todos los parqueaderos.
    /// </summary>
    /// <param name="paginationDto">
    /// DTO que contiene la página y el límite de resultados.
    /// </param>
    /// <returns>
    /// Un objeto con la metadata de la paginación y los parqueaderos existentes
    /// </returns>
    /// <response code="200">Devuelve el listado de los parqueaderos.</response>
    /// <response code="400">Si el DTO tiene datos inválidos</response>
    /// <response code="401">Si el usuario no está autenticado.</response>
    /// <response code="403">Si el usuario no tiene permisos de administrador.</response>
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
