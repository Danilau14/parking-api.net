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
    private readonly IMediator _mediator;
    private readonly IS3Service _s3Service;

    public ParkingsLotsController(IMediator mediator, IS3Service s3Service) 
    {
        _mediator = mediator;
        _s3Service = s3Service;
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
        return Ok(parkingLotSaved);
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
        var command = new FindOneByIdParkingLotQuery(id);
        var parkingLotFound = await _mediator.Send(command);
        return Ok(parkingLotFound);
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
        var command = new UpdateParkingLotCommand(id, updatedParkingLotDto);    
        var parkingLotUpdated = await _mediator.Send(command);
        return Ok(parkingLotUpdated);
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
        var command = new RemoveParkingLotCommand(id);

        var parkingLot = await _mediator.Send(command);

        return Ok(parkingLot);
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
        var command = new FindParkingsLotQuery(paginationDto.Page, paginationDto.Limit);

        var listParkingDto = await _mediator.Send(command);

        return Ok(listParkingDto);
    }

    [HttpPost("upload-csv")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadCsv([FromForm] UploadCsvDto dto)
    {
       if (dto.File == null || Path.GetExtension(dto.File.FileName).ToLower() != ".csv")
        {
            return BadRequest("Solo se permiten archivos CSV.");
        }

        var key = $"parking-lot/{Guid.NewGuid()}_{dto.File.FileName}";

        await _s3Service.UploadFileAsync("parking-api", key, dto.File.OpenReadStream());

        var fileUrl = $"http://localhost:4566/parking-api/{key}";

        return Ok(new { message = "Archivo recibido correctamente.", url = fileUrl });
    }
}
