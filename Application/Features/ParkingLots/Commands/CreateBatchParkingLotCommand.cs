namespace ParkingApi.Application.Features.ParkingLots.Commands;

public record CreateBatchParkingLotCommand(IFormFile Csv) : IRequest<bool>;
