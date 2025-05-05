using ParkingApi.Core.Interfaces;

namespace ParkingApi.Application.Features.ParkingHistories.Handlers;

public class CreateParkingHistoryHandler : IRequestHandler<CreateParkingHistoryCommand, ParkingHistory>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IUserContextService _userContextService;

    public CreateParkingHistoryHandler(
        IUnitOfWork unitOfWork, 
        IMediator mediator,
        IUserContextService userContextService
    )
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _userContextService = userContextService;
    }

    public async Task<ParkingHistory> Handle(CreateParkingHistoryCommand command, CancellationToken cancellationToken)
    {
        var userId = _userContextService.GetCurrentUserId();
        
        var parkingLot = await _unitOfWork.ParkingLotRepository.FindByParkingLotAndUser(command.ParkingLotId, userId.Value);

        if (parkingLot == null)
        {
            throw new EipexException(new ErrorResponse
            {
                Message = "This parking lot does not belong to the member",
                ErrorCode = ErrorsCodeConstants.PARKINGLOT_INVALID
            }, HttpStatusCode.Unauthorized);
        }

        var vehicle = await _unitOfWork.VehicleRepository.FindOneVehicleByLicencePlate(command.LicensePlate);

        if (null == vehicle)
        {
            return await this.CreateParkingHistoryForNewVehicle(
                command.LicensePlate, 
                command.ParkingLotId,
                parkingLot
            );
        }

        if (vehicle.IsParked)
        {
            throw new EipexException(new ErrorResponse
            {
                Message = "Unable to Register Entry, the license plate already exists in this or another parking lot.",
                ErrorCode = ErrorsCodeConstants.VEHICLE_INVALID
            }, HttpStatusCode.Unauthorized
            );
        }
        var newParkingHistory = await this.CreateParkingHistoryForExistingVehicle(
                    vehicle,
                    parkingLot
                );

        if (parkingLot.User?.Email != null)
        {
            var email = new SendEmailEvent(
                     parkingLot.User.Email,
                    "Vehicle in Parking lot",
                     $"Vehicle with LicensePlate {command.LicensePlate} in ParkingLot {command.ParkingLotId}"
                );

            await _mediator.Publish(email);
        }
        return newParkingHistory;
    }

    private async Task<ParkingHistory> CreateParkingHistoryForNewVehicle(
            string licensePlate,
            int parkingLotId,
            ParkingLot parkingLot
        )
    {
        if (parkingLot.FreeSpaces == 0)
        {
            throw new BadHttpRequestException("Parking lot full");
        }

        var newVehicle = new Vehicle
        {
            IsParked = true,
            LicensePlate = licensePlate,
            ParkingHistories = []
        };

        await _unitOfWork.VehicleRepository.CreateVehicle(newVehicle);

        parkingLot.FreeSpaces -= 1;

        await _unitOfWork.ParkingLotRepository.UpdatedParkingLot(parkingLot);

        var newParkingHistory = new ParkingHistory
        {
            ParkingLot = parkingLot,
            Vehicle = newVehicle,
            ParkingLotId = parkingLot.Id,
            VehicleId = newVehicle.Id
        };

        var parkingHistorySaved = await _unitOfWork.ParkingHistoryRepository.CreateParkingHistory(newParkingHistory);

        if (parkingLot.User?.Email != null)
        {
            var email = new SendEmailEvent(
                     parkingLot.User.Email,
                     "Vehicle in Parking lot",
                     $"Vehicle with LicensePlate {licensePlate} in ParkingLot {parkingLotId}"
                );

            await _mediator.Publish(email);
        }

        return parkingHistorySaved;
    }

    private async Task<ParkingHistory> CreateParkingHistoryForExistingVehicle(
            Vehicle vehicle,
            ParkingLot parkingLot
        )
    {
        if (parkingLot.FreeSpaces == 0)
        {
            throw new EipexException(new ErrorResponse
            {
                Message = "Parking Lot full",
                ErrorCode = ErrorsCodeConstants.PARKINGLOT_INVALID
            }, HttpStatusCode.BadRequest
            );
        }

        var parkingHistoryOpen = await _unitOfWork.ParkingHistoryRepository.FindOneParkingHistoryOpen(
               vehicle.Id,
               parkingLot.Id
            );

        if (null == parkingHistoryOpen && vehicle.IsParked)
        {
            throw new EipexException(new ErrorResponse
            {
                Message = "Vehicle in other Parking lot",
                ErrorCode = ErrorsCodeConstants.VEHICLE_INVALID
            }, HttpStatusCode.BadRequest
            );
        }

        vehicle.IsParked = true;

        await _unitOfWork.VehicleRepository.UpdateVehicle(vehicle);

        parkingLot.FreeSpaces -= 1;

        await _unitOfWork.ParkingLotRepository.UpdatedParkingLot(parkingLot);

        var newParkingHistory = new ParkingHistory
        {
            ParkingLot = parkingLot,
            Vehicle = vehicle,
            ParkingLotId = parkingLot.Id,
            VehicleId = vehicle.Id,
        };

        var parkingHistorySaved = await _unitOfWork.ParkingHistoryRepository.CreateParkingHistory(newParkingHistory);

        return parkingHistorySaved;
    }
}
