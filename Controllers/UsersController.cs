namespace ParkingApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IS3Service _s3Service;

    public UsersController(IMediator mediator, IS3Service s3Service)
    {
        _mediator = mediator;
        _s3Service = s3Service;
    }

    /// <summary>
    /// Registra un nuevo usuario.
    /// </summary>
    /// <param name="data">
    /// DTO que contiene los detalles del usuario a crear, como nombre, correo y contraseña.
    /// </param>
    /// <returns>
    /// Un mensaje que indica que el usuario fue creado exitosamente.
    /// </returns>
    /// <response code="200">Usuario creado exitosamente.</response>
    /// <response code="400">Si el DTO tiene datos inválidos o el proceso de creación falla.</response>
    /// <exception cref="EipexException">
    /// Se lanza si ocurre algún error al crear el usuario.
    /// </exception>
    /// <remarks>
    /// Este endpoint permite crear un nuevo usuario en el sistema.
    /// </remarks>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto data)
    {
        var command = new RegisterUserCommand(data.Email, data.Password, data.Role);
        await _mediator.Send(command);
        return Created();
    }

    [HttpPost("send-email-partner")]
    public async Task<IActionResult> SendEmail([FromBody] EmailForUserDto emailForUser)
    {
        var notification = new SendEmailEvent(emailForUser.Email, emailForUser.Subject, emailForUser.Message);
        await _mediator.Publish(notification);
        var buckets = await _s3Service.ListBucketsAsync();
        return Ok( new { Message = buckets.First() });
    }
}