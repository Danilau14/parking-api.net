namespace ParkingApi.Application.Features.ParkingLots.Queries;

public record  FindOneByIdParkingLotQuery(int Id) : IRequest<ParkingLotDto>;
