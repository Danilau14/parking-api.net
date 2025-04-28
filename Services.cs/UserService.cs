namespace ParkingApi.Services.cs;

public class UserService : IUserServiceInterface
{
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;


    public UserService(IUserRepository userRepository, IJwtTokenGenerator tokenGenerator)
    {
        _passwordHasher = new PasswordHasher<User>();
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<User>CreateUserAsync(User user)
    {
        var hashedPassword = _passwordHasher.HashPassword(user, user.Password);
        user.Password = hashedPassword; 
        return await _userRepository.CreateUser(user);
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
