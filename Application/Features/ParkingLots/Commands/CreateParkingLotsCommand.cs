namespace ParkingApi.Application.Features.ParkingLots.Commands;

public record CreateParkingLotsCommand(int Size, float CostPerHour, string? PartnerId = null) : IRequest<ParkingLotDynamo>;
