namespace ParkingApi.Application.Features.ParkingHistories.Handlers;

public class FindParkedVehiclesHandler : IRequestHandler<FindParkedVehiclesQuery, PagedResult<ParkingHistoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FindParkedVehiclesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork; 
        _mapper = mapper;
    }

    public async Task<PagedResult<ParkingHistoryDto>> Handle(FindParkedVehiclesQuery query, CancellationToken cancellation)
    {
        var (parkingsLot, totalCount) = await _unitOfWork.ParkingHistoryRepository
            .FindVehiclesByParkingLot(
                            query.Page,
                            query.Limit
                        );

        var parkingHistoryDtos = _mapper.Map<List<ParkingHistoryDto>>(parkingsLot);

        return new PagedResult<ParkingHistoryDto>(parkingHistoryDtos, totalCount, query.Page, query.Limit);
    }
}
