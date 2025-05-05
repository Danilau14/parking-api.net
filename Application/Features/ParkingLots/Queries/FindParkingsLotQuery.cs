namespace ParkingApi.Application.Features.ParkingLots.Queries;

public record FindParkingsLotQuery(int Page=1, int Limit = 10) : IRequest<PagedResult<ParkingLotDto>>;
