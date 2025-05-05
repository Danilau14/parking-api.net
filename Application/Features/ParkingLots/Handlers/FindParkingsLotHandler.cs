using ParkingApi.Core.Interfaces;
using ParkingApi.Core.Models;

namespace ParkingApi.Application.Features.ParkingLots.Handlers;

public class FindParkingsLotHandler : IRequestHandler<FindParkingsLotQuery, PagedResult<ParkingLotDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FindParkingsLotHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<ParkingLotDto>> Handle(FindParkingsLotQuery query, CancellationToken cancellationToken)
    {
        var (parkingsLot, totalCount) = await _unitOfWork.ParkingLotRepository.FindAndCountAsync(
                query.Page,
                query.Limit
            );

        var parkingsLotDto = _mapper.Map<List<ParkingLotDto>>(parkingsLot);

        return new PagedResult<ParkingLotDto>(parkingsLotDto, totalCount, query.Page, query.Limit);
    }
}
