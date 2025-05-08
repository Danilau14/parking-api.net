namespace ParkingApi.Application.Features.ParkingLots.Handlers;

public class FindOneByIdParkingLotHandler : IRequestHandler<FindOneByIdParkingLotQuery, ParkingLotDynamo>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public FindOneByIdParkingLotHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ParkingLotDynamo> Handle(FindOneByIdParkingLotQuery query, CancellationToken cancellationToken)
    {
        var parkingLot = await _unitOfWork.ParkingLotRepositoryDynamo.GetByIdAsync(query.Id);

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
