namespace ParkingApi.Application.Features.Users.Handlers;
public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, User>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PasswordHasher<User> _passwordHasher;

    public RegisterUserHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userExist = await _unitOfWork.UserRepository.FindByEmail(request.Email);

        if (userExist != null)
        {
            throw new EipexException(new ErrorResponse
            {
                Message = "Exist User with this email",
                ErrorCode = ErrorsCodeConstants.USER_NOT_SAVE
            }, HttpStatusCode.BadRequest
            );
        }

        var newUser = new User()
        {
            Email = request.Email,
            Password = request.Password,
            Role = request.Role,
            ParkingLots = new List<ParkingLot>()
        };

        var hashedPassword = _passwordHasher.HashPassword(newUser, newUser.Password);
        newUser.Password = hashedPassword;

        var (isSaved, response) = await _unitOfWork.UserRepository.CreateUser(newUser);

        if (isSaved < 1)
        {
            throw new EipexException(new ErrorResponse
            {
                Message = response,
                ErrorCode = ErrorsCodeConstants.USER_NOT_SAVE
            }, HttpStatusCode.BadRequest
            );
        }

        return newUser;
    }
}
