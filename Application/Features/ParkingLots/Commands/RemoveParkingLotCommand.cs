using ParkingApi.Core.Models;

namespace ParkingApi.Application.Features.ParkingLots.Commands;

public record RemoveParkingLotCommand(int parkingLotId) : IRequest<ParkingLotDto>;
