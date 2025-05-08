namespace ParkingApi.Application.Features.ParkingLots.Handlers;

public class CreateParkingLotsHandler : IRequestHandler<CreateParkingLotsCommand, ParkingLotDynamo>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateParkingLotsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ParkingLotDynamo> Handle(CreateParkingLotsCommand command, CancellationToken cancellation)
    {
        var parkingLot = new ParkingLotDynamo
        {
            CostPerHour = command.CostPerHour,
            FreeSpaces = command.Size,
            Size = command.Size,
        };

        if (command.PartnerId != null)
        {
            var partnerExist = await _unitOfWork.UserRepositoryDynamo.GetByIdAsync(command.PartnerId);

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

        var parkingLotSaved = await _unitOfWork.ParkingLotRepositoryDynamo.CreateParkingLot(parkingLot);

        return parkingLotSaved;
    }
}
