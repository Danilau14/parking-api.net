namespace ParkingApi.Application.Features.ParkingLots.Handlers;

public class RemoveParkingLotHandler : IRequestHandler<RemoveParkingLotCommand, ParkingLotDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RemoveParkingLotHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ParkingLotDto> Handle(RemoveParkingLotCommand command, CancellationToken cancellationToken)
    {
        var parkingLot = await _unitOfWork.ParkingLotRepository.GetByIdAsync(command.parkingLotId);

        if (parkingLot == null)
        {
            throw new EipexException(new ErrorResponse
            {
                Message = "Parking dont found",
                ErrorCode = ErrorsCodeConstants.PARKINGLOT_NOT_FOUND
            }, HttpStatusCode.NotFound
            );
        }

        parkingLot.RecycleBin = true;

        var parkingLotRemove = await _unitOfWork.ParkingLotRepository.UpdatedParkingLot(parkingLot);

        return _mapper.Map<ParkingLotDto>(parkingLotRemove);
    }
}