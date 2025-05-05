namespace ParkingApi.Application.Features.ParkingHistories.Queries;

public record FindParkedVehiclesQuery(int Page = 1, int Limit = 10) : IRequest<PagedResult<ParkingHistoryDto>>;
