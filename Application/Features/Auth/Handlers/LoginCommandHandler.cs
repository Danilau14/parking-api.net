using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;

namespace ParkingApi.Application.Features.Auth.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, string?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly PasswordHasher<User> _passwordHasher;

    public LoginCommandHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator tokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = new PasswordHasher<User>();
        _tokenGenerator = tokenGenerator;
    }

    public async Task<string?> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.FindByEmail(command.Email);
        if (user == null)
        {
            throw new EipexException(new ErrorResponse
            {
                Message = "Invalid Credentials",
                ErrorCode = ErrorsCodeConstants.LOGIN_INVALID
            }, HttpStatusCode.BadRequest
            );
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, command.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new EipexException(new ErrorResponse
                {
                    Message = "Invalid Credentials",
                    ErrorCode = ErrorsCodeConstants.LOGIN_INVALID
                }, HttpStatusCode.BadRequest
            );
        }

        return _tokenGenerator.GenerateToken(user);
    }
}
