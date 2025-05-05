using ParkingApi.Core.Models;

namespace ParkingApi.Application.Features.ParkingLots.Commands;

public record CreateParkingLotsCommand(int Size, float CostPerHour, int? PartnerId = null) : IRequest<ParkingLotDto>;
