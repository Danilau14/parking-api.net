namespace ParkingApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserServiceInterface _userService;
    private readonly IEmailService _emailService;   
    private readonly IMapper _mapper;

    public UsersController(
        IUserServiceInterface userService, 
        IMapper mapper,
        IEmailService emailService
        )
    {
        _userService = userService;
        _mapper = mapper;
        _emailService = emailService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto data)
    {
        var user = _mapper.Map<User>(data);

        await _userService.CreateUserAsync(user);
      
        return Ok("Usuario creado");
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

            return Ok( new
                {
                    Message = "Send email",
                }
            );
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = ex.Message,
            }
           );
        } 
    }
}