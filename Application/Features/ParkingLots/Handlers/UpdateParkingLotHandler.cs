namespace ParkingApi.Application.Features.ParkingLots.Handlers;

public class UpdateParkingLotHandler : IRequestHandler<UpdateParkingLotCommand, ParkingLotDynamo>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdateParkingLotHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ParkingLotDynamo> Handle(UpdateParkingLotCommand command, CancellationToken cancellationToken)
    {
        var parkingLot = await _unitOfWork.ParkingLotRepositoryDynamo.GetByIdAsync(command.id);

        if (parkingLot == null)
        {
            throw new EipexException(new ErrorResponse
            {
                Message = "Parking dont found",
                ErrorCode = ErrorsCodeConstants.PARKINGLOT_NOT_FOUND
            }, HttpStatusCode.NotFound
            );
        }

        var updatedParkingLotDto = command.UpdatedParkingLotDto;

        if (updatedParkingLotDto.PartnerId != null)
        {
            var partner = await _unitOfWork.UserRepositoryDynamo.GetByIdAsync(updatedParkingLotDto.PartnerId);

            if (partner == null)
            {
                throw new EipexException(new ErrorResponse
                {
                    Message = "Invalid PartnerId",
                    ErrorCode = ErrorsCodeConstants.PARKINGLOT_INVALID
                }, HttpStatusCode.NotFound
                );
            }

            parkingLot.UserId = partner.Id;
        }

        if (updatedParkingLotDto.Size.HasValue)
        {
            if (updatedParkingLotDto.Size.Value > parkingLot.Size)
            {
                parkingLot.FreeSpaces = parkingLot.FreeSpaces + (updatedParkingLotDto.Size.Value - parkingLot.Size);
                parkingLot.Size = updatedParkingLotDto.Size.Value;
            }

            if (updatedParkingLotDto.Size.Value < parkingLot.Size)
            {
                if (parkingLot.FreeSpaces == 0 || parkingLot.Size - parkingLot.FreeSpaces > updatedParkingLotDto.Size.Value)
                {
                    throw new EipexException(new ErrorResponse
                    {
                        Message = "The size of the parking lot cannot be less than the number of current vehicles.",
                        ErrorCode = ErrorsCodeConstants.PARKINGLOT_CONFLICT
                    }, HttpStatusCode.Conflict
                    );
                }

                parkingLot.FreeSpaces = parkingLot.FreeSpaces - (parkingLot.Size - updatedParkingLotDto.Size.Value);
                parkingLot.Size = updatedParkingLotDto.Size.Value;
            }
        }

        if (updatedParkingLotDto.CostPerHour.HasValue)
        {
            parkingLot.CostPerHour = updatedParkingLotDto.CostPerHour.Value;
        }

        var parkingLotUpdate = await _unitOfWork.ParkingLotRepositoryDynamo.UpdatedParkingLot(parkingLot);

        return parkingLotUpdate;
        }
    }
