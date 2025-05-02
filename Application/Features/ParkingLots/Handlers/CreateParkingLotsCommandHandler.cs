namespace ParkingApi.Application.Features.ParkingLots.Handlers;

public class CreateParkingLotsCommandHandler : IRequestHandler<CreateParkingLotsCommand, ParkingLot>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateParkingLotsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ParkingLot> Handle(CreateParkingLotsCommand command, CancellationToken cancellation)
    {
        var parkingLot = new ParkingLot
        {
            CostPerHour = command.CostPerHour,
            FreeSpaces = command.Size,
            Size = command.Size,
            ParkingHistories = new List<ParkingHistory>()
        };

        if (command.PartnerId != null)
        {
            var partnerExist = await _unitOfWork.UserRepository.GetByIdAsync(command.PartnerId.Value);

            if (null == partnerExist)
            {
                throw new EipexException(new ErrorResponse
                {
                    Message = "Partner not found",
                    ErrorCode = ErrorsCodeConstants.PARTNER_INVALID
                }, HttpStatusCode.NotFound
                );
            }
            parkingLot.UserId = partnerExist.Id;
        }

        await _unitOfWork.ParkingLotRepository.CreateParkingLot(parkingLot);

        return parkingLot;
    }
}
