namespace ParkingApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserServiceInterface _userService;
    private readonly IEmailService _emailService;   
    private readonly IMapper _mapper;
    private readonly IRabbitMQService _rabbitmqService;

    public UsersController(
        IUserServiceInterface userService, 
        IMapper mapper,
        IEmailService emailService,
        IRabbitMQService rabbitmqService
    )
    {
        _userService = userService;
        _mapper = mapper;
        _emailService = emailService;
        _rabbitmqService = rabbitmqService;
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
        var user = _mapper.Map<User>(data);

        var message = new MessageDto
        {
            Entity = user.GetType().Name,
            Action = Actions.CREATE,
            State = true
        };

        try
        {
            await _userService.CreateUserAsync(user);
            await _rabbitmqService.PublishMessage(message);

            return Created();

        } catch (Exception ex) {
            message.Response = ex.Message;
            message.State = false;
            await _rabbitmqService.PublishMessage(message);
            throw new EipexException(new ErrorResponse
                {
                    Message = ex.Message,
                    ErrorCode = ErrorsCodeConstants.USER_NOT_SAVE
                }, HttpStatusCode.BadRequest
            );
        }
    }

    [HttpPost("send-email-partner")]
    public async Task<IActionResult> SendEmail([FromBody] EmailForUserDto emailForUser)
    {
        try
        {
            await _emailService.SendEmailAsync(
                    emailForUser.Email,
                    emailForUser.Subject,
                    emailForUser.Message
                );

            return Ok( new { Message = "Send email" });
        }
        catch (Exception ex)
        {
            throw new EipexException(new ErrorResponse
                {
                    Message = ex.Message,
                    ErrorCode = ErrorsCodeConstants.EMAIL_FAILED
                }, HttpStatusCode.BadRequest
            );
        } 
    }
}