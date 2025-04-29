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

    /// <summary>
    /// Autentica al usuario y genera un token JWT.
    /// </summary>
    /// <param name="loginDto">
    /// DTO que contiene el correo electrónico y la contraseña del usuario.
    /// </param>
    /// <returns>
    /// Un objeto JSON que contiene el token si la autenticación es exitosa.
    /// </returns>
    /// <response code="200">Devuelve el token JWT.</response>
    /// <response code="400">Si las credenciales son inválidas.</response>
    /// <exception cref="EipexException">
    /// Se lanza si el usuario proporciona credenciales incorrectas.
    /// </exception>
    /// <remarks>
    /// Este endpoint valida las credenciales y devuelve un token JWT que debe ser usado en las peticiones autenticadas.
    /// </remarks>
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

    /// <summary>
    /// Desloguea al usuario actual y revoca su token JWT.
    /// </summary>
    /// <returns>
    /// Un objeto JSON que indica que el cierre de sesión fue exitoso.
    /// </returns>
    /// <response code="200">Devuelve un mensaje de cierre de sesión exitoso.</response>
    /// <response code="400">Si el token ya ha sido revocado o no se encuentra en la cabecera.</response>
    /// <exception cref="EipexException">
    /// Se lanza si no se proporciona un token en la cabecera o si el token es inválido.
    /// </exception>
    /// <remarks>
    /// Este endpoint almacena el token en una lista de tokens revocados para evitar su reutilización.
    /// </remarks>
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

        return Ok( new {Message = "Logged out successfully" });
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
