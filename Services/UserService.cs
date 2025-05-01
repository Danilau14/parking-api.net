using Mono.TextTemplating;

namespace ParkingApi.Services;

public class UserService : IUserServiceInterface
{
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IRabbitMQMessageBuilder _rabbitMQMessageBuilder;
    
    public UserService(
        IUserRepository userRepository, 
        IJwtTokenGenerator tokenGenerator,
        IRabbitMQMessageBuilder rabbitMQMessageBuilder
    )
    {
        _passwordHasher = new PasswordHasher<User>();
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
        _rabbitMQMessageBuilder = rabbitMQMessageBuilder;
    }

    public async Task<User>CreateUserAsync(User user)
    {
        var userExist =  await _userRepository.FindByEmail(user.Email);

        if (userExist != null)
        {
            throw new EipexException(new ErrorResponse
                {
                    Message = "Exist User with this email",
                    ErrorCode = ErrorsCodeConstants.USER_NOT_SAVE
                }, HttpStatusCode.BadRequest
            );
        } 
        var hashedPassword = _passwordHasher.HashPassword(user, user.Password);
        user.Password = hashedPassword;

        var (isSaved, response) = await _userRepository.CreateUser(user);

         if(isSaved<1)
         {
            throw new EipexException(new ErrorResponse
                {
                    Message = response,
                    ErrorCode = ErrorsCodeConstants.USER_NOT_SAVE
                }, HttpStatusCode.BadRequest
            );
         }

         return user;
    }

    public async Task<string?> LoginAsync(string email, string password)
    {
        var user = await _userRepository.FindByEmail(email);
        if (user == null) return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        if (result == PasswordVerificationResult.Failed) return null;

        return _tokenGenerator.GenerateToken(user);
    }
}
