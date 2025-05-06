namespace ParkingApi.Application.Features.ParkingLots.Handlers;

public class CreateParkingLotsHandler : IRequestHandler<CreateParkingLotsCommand, ParkingLotDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateParkingLotsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ParkingLotDto> Handle(CreateParkingLotsCommand command, CancellationToken cancellation)
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

        var parkingLotSaved = await _unitOfWork.ParkingLotRepository.CreateParkingLot(parkingLot);

        return _mapper.Map<ParkingLotDto>(parkingLotSaved);
    }
}
