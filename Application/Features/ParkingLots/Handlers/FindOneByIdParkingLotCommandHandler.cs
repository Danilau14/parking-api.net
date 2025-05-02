namespace ParkingApi.Application.Features.ParkingLots.Handlers;

public class FindOneByIdParkingLotCommandHandler : IResult<FindOneByIdParkingLotCommand, ParkingLot>
{
    private readonly IUnitOfWork _unitOfWork;
    public FindOneByIdParkingLotCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ParkingLot> Handle(FindOneByIdParkingLotCommand command, CancellationToken cancellationToken)
    {
        var parkingLot = await _unitOfWork.ParkingLotRepository.GetByIdAsync(command.Id);

        if (parkingLot == null)
        {
            throw new EipexException(new ErrorResponse
                {
                    Message = "Parking dont found",
                    ErrorCode = ErrorsCodeConstants.PARKINGLOT_NOT_FOUND
                }, HttpStatusCode.NotFound
            );
        }

        return parkingLot;
    }
}
