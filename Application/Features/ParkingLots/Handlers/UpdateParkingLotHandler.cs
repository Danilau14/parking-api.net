namespace ParkingApi.Application.Features.ParkingLots.Handlers;

public class UpdateParkingLotHandler : IRequestHandler<UpdateParkingLotCommand, ParkingLotDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdateParkingLotHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ParkingLotDto> Handle(UpdateParkingLotCommand command, CancellationToken cancellationToken)
    {
        var parkingLot = await _unitOfWork.ParkingLotRepository.GetByIdAsync(command.id);

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

        if (updatedParkingLotDto.PartnerId.HasValue)
        {
            var partner = await _unitOfWork.UserRepository.GetByIdAsync(updatedParkingLotDto.PartnerId.Value);

            if (partner == null)
            {
                throw new EipexException(new ErrorResponse
                {
                    Message = "Invalid PartnerId",
                    ErrorCode = ErrorsCodeConstants.PARKINGLOT_INVALID
                }, HttpStatusCode.NotFound
                );
            }

            parkingLot.User = partner;
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

        var parkingLotUpdate = await _unitOfWork.ParkingLotRepository.UpdatedParkingLot(parkingLot);

        return _mapper.Map<ParkingLotDto>(parkingLotUpdate);
        }
    }
