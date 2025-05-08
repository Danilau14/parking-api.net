namespace ParkingApi.Application.Features.ParkingLots.Queries;

public record  FindOneByIdParkingLotQuery(string Id) : IRequest<ParkingLotDynamo>;
