namespace ParkingApi.Application.Features.Users.Handlers;
public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserDynamo>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PasswordHasher<UserDynamo> _passwordHasher;

    public RegisterUserHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = new PasswordHasher<UserDynamo>();
    }

    public async Task<UserDynamo> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userExist = await _unitOfWork.UserRepositoryDynamo.FindByEmail(request.Email);

        if (userExist != null)
        {
            throw new EipexException(new ErrorResponse
            {
                Message = "Exist User with this email",
                ErrorCode = ErrorsCodeConstants.USER_NOT_SAVE
            }, HttpStatusCode.BadRequest
            );
        }

        var newUser = new UserDynamo()
        {
            Email = request.Email,
            Password = request.Password,
            Role = request.Role,
        };

        var hashedPassword = _passwordHasher.HashPassword(newUser, newUser.Password);
        newUser.Password = hashedPassword;

        var (isSaved, response) = await _unitOfWork.UserRepositoryDynamo.CreateUser(newUser);

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
