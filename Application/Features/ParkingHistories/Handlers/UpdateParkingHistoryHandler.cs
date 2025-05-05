using ParkingApi.Core.Interfaces;
using ParkingApi.Core.Models;

namespace ParkingApi.Application.Features.ParkingHistories.Handlers;

public class UpdateParkingHistoryHandler : IRequestHandler<UpdateParkingHistoryCommand, ParkingHistoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateParkingHistoryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) 
    {  
        _unitOfWork = unitOfWork; 
        _mapper = mapper;
    }

    public async Task<ParkingHistoryDto> Handle(UpdateParkingHistoryCommand command, CancellationToken cancellation)
    {
        {
            var parkingLot = await _unitOfWork.ParkingLotRepository.GetByIdAsync(command.ParkingLotId);


            if (parkingLot == null)
            {
                throw new EipexException(new ErrorResponse
                {
                    Message = "ParkingLot not found",
                    ErrorCode = ErrorsCodeConstants.PARKINGLOT_NOT_FOUND
                }, HttpStatusCode.NotFound
                );
            }

            var vehicle = await _unitOfWork.VehicleRepository.FindOneVehicleByLicencePlate(command.LicensePlate);

            if (vehicle == null)
            {
                throw new EipexException(new ErrorResponse
                {
                    Message = "Vehicle not found",
                    ErrorCode = ErrorsCodeConstants.VEHICLE_NOT_FOUND
                }, HttpStatusCode.NotFound
                );
            }

            var parkingHistoryOpen = await _unitOfWork.ParkingHistoryRepository.FindOneParkingHistoryOpen(
                vehicle.Id,
                parkingLot.Id
            );

            if (parkingHistoryOpen == null)
            {
                if (vehicle.IsParked)
                {
                    throw new EipexException(new ErrorResponse
                    {
                        Message = "Unable to Register Entry, the license plate already exists in this or another parking lot.",
                        ErrorCode = ErrorsCodeConstants.VEHICLE_INVALID
                    }, HttpStatusCode.BadRequest
                    );

                }
                throw new EipexException(new ErrorResponse
                {
                    Message = "Unable to Check Out, there is no license plate in the parking lot.",
                    ErrorCode = ErrorsCodeConstants.VEHICLE_INVALID
                }, HttpStatusCode.BadRequest
                );
            }

            parkingHistoryOpen.CheckOutDate = DateTime.UtcNow;

            var updatedParkingHistory = await _unitOfWork.ParkingHistoryRepository.UpdateTimeAndCostInParkingHistory(
                    parkingLot.CostPerHour,
                    parkingHistoryOpen
                );

            vehicle.IsParked = false;

            await _unitOfWork.VehicleRepository.UpdateVehicle(vehicle);

            parkingLot.FreeSpaces += 1;

            await _unitOfWork.ParkingLotRepository.UpdatedParkingLot(parkingLot);

           var dto = _mapper.Map<ParkingHistoryDto>(updatedParkingHistory);

           return dto;
        }
    }
}
