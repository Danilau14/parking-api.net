namespace ParkingApi.Application.Features.Auth.Handlers;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        bool result = await _unitOfWork.RevokedTokenRepository.SaveTokenRevoked(
            new RevokedToken { Token = command.token }
            );

        if (!result)
        {
            throw new EipexException(new ErrorResponse
            {
                Message = "The token could not be revoked.",
                ErrorCode = ErrorsCodeConstants.LOGOUT_INVALID
            }, HttpStatusCode.BadRequest
            );
        }

        return result;
    }
}
