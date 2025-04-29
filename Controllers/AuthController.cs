namespace ParkingApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserServiceInterface _userService;
    private readonly IRevokedTokenRepository _revokedTokenRepository;

    public AuthController(
        IUserServiceInterface userService,
        IRevokedTokenRepository revokedTokenRepository
        )
    {
        _userService = userService;
        _revokedTokenRepository = revokedTokenRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _userService.LoginAsync(loginDto.Email, loginDto.Password);

        if (token == null)
        {
            throw new EipexException(new ErrorResponse
                {
                    Message = "Invalid Credentials",
                    ErrorCode = ErrorsCodeConstants.LOGIN_INVALID
                }, HttpStatusCode.BadRequest
            );
        }

        return Ok(new { token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var bearerToken  =  Request.Headers.Authorization.ToString();
        if (null == bearerToken) {
            throw new EipexException(new ErrorResponse
                {
                    Message = "No token provided",
                    ErrorCode = ErrorsCodeConstants.LOGOUT_INVALID
                }, HttpStatusCode.BadRequest
            );
        }

        var jwt = bearerToken.Split(' ')[1];

        bool result = await _revokedTokenRepository.SaveTokenRevoked(
            new RevokedToken { Token = jwt }
            );

        if (!result) {
            throw new EipexException(new ErrorResponse
                {
                    Message = "The token could not be revoked.",
                    ErrorCode = ErrorsCodeConstants.LOGOUT_INVALID
                }, HttpStatusCode.BadRequest
            );
        } 

        return Ok("Logged out successfully");
     }

    [Authorize]
    [HttpGet("me")]
    [Authorize(Policy = "PartnerOnly")]
    public IActionResult GetCurrentUserInfo()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            Email = email,
            Role = role
        });
    }
}
