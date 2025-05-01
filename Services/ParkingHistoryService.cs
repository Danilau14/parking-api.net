namespace ParkingApi.Services;

public class ParkingHistoryService : IParkingHistoryService
{
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IParkingHistoryRepository _parkingHistoryRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<ParkingHistoryService> _logger;
    private readonly IRabbitMQSendMail _rabbitMQSendMail;

    public ParkingHistoryService(
        IParkingLotRepository parkingLotRepository,
        IVehicleRepository vehicleRepository,
        IParkingHistoryRepository parkingHistoryRepository,
        IEmailService emailService,
        ILogger<ParkingHistoryService> logger,
        IRabbitMQSendMail rabbitMQSendMail
        )
    {
        _parkingLotRepository = parkingLotRepository;
        _vehicleRepository = vehicleRepository;
        _parkingHistoryRepository = parkingHistoryRepository;
        _emailService = emailService;
        _logger = logger;
        _rabbitMQSendMail = rabbitMQSendMail;
    }

    public async Task<ParkingHistory> CreateParkingHistory(
        CreateParkingHistoryDto createParkingHistoryDto,
        int partnerId
        )
    {
        var parkingLot = await _parkingLotRepository.FindByParkingLotAndUser(
            createParkingHistoryDto.ParkingLotId, partnerId
            );

        if (parkingLot == null)
        {
            throw new EipexException(new ErrorResponse
            {
                Message = "This parking lot does not belong to the member",
                ErrorCode = ErrorsCodeConstants.PARKINGLOT_INVALID
            }, HttpStatusCode.Unauthorized);
        }

        var vehicle = await _vehicleRepository.FindOneVehicleByLicencePlate(
            createParkingHistoryDto.LicensePlate
            );

        if (null == vehicle)
        {
            return await this.CreateParkingHistoryForNewVehicle(
                    createParkingHistoryDto,
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
            try
            {
                await _rabbitMQSendMail.PublishAuditMessageAsync(
                        parkingLot.User.Email,
                        "Vehicle in Parking lot",
                        $"Vehicle with LicensePlate {vehicle.LicensePlate} in ParkingLot {parkingLot.Id}"
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {Recipient}", parkingLot.User.Email);
            }
        }
        return newParkingHistory;
    }

    private async Task<ParkingHistory> CreateParkingHistoryForNewVehicle(
            CreateParkingHistoryDto createParkingHistoryDto,
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
            LicensePlate = createParkingHistoryDto.LicensePlate,
            ParkingHistories = []
        };

        await _vehicleRepository.CreateVehicle(newVehicle);

        parkingLot.FreeSpaces -= 1;

        await _parkingLotRepository.UpdatedParkingLot(parkingLot);

        var newParkingHistory = new ParkingHistory
        {
            ParkingLot = parkingLot,
            Vehicle = newVehicle,
            ParkingLotId = parkingLot.Id,
            VehicleId = newVehicle.Id
        };

        var parkingHistorySaved = await _parkingHistoryRepository.CreateParkingHistory(newParkingHistory);

        if (parkingLot.User?.Email != null)
        {
            try
            {
                await _rabbitMQSendMail.PublishAuditMessageAsync(
                        parkingLot.User.Email,
                        "Vehicle in Parking lot",
                        $"Vehicle with LicensePlate {newVehicle.LicensePlate} in ParkingLot {parkingLot.Id}"
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {Recipient}", parkingLot.User.Email);
            }
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

        var parkingHistoryOpen = await _parkingHistoryRepository.FindOneParkingHistoryOpen(
               vehicle.Id,
               parkingLot.Id
            );

        if (null == parkingHistoryOpen && vehicle.IsParked) {
            throw new EipexException(new ErrorResponse
                {
                    Message = "Vehicle in other Parking lot",
                    ErrorCode = ErrorsCodeConstants.VEHICLE_INVALID
                }, HttpStatusCode.BadRequest
            );
        }

        vehicle.IsParked = true;

        await _vehicleRepository.UpdateVehicle( vehicle );

        parkingLot.FreeSpaces -= 1;

        await _parkingLotRepository.UpdatedParkingLot(parkingLot);

        var newParkingHistory = new ParkingHistory
        {
            ParkingLot = parkingLot,
            Vehicle = vehicle,
            ParkingLotId = parkingLot.Id,
            VehicleId = vehicle.Id,
        };

        var parkingHistorySaved = await _parkingHistoryRepository.CreateParkingHistory(newParkingHistory);

        return parkingHistorySaved;
    }

    public async Task<ParkingHistory> CloseParkingHistory(CreateParkingHistoryDto createParkingHistoryDto)
    {
        var parkingLot = await _parkingLotRepository.GetByIdAsync(createParkingHistoryDto.ParkingLotId);


        if (parkingLot == null)
        {
            throw new EipexException(new ErrorResponse
                {
                    Message = "ParkingLot not found",
                    ErrorCode = ErrorsCodeConstants.PARKINGLOT_NOT_FOUND
                }, HttpStatusCode.NotFound
            );
        }

        var vehicle = await _vehicleRepository.FindOneVehicleByLicencePlate(
            createParkingHistoryDto.LicensePlate
            );

        if (vehicle == null)
        {
            throw new EipexException(new ErrorResponse
                {
                    Message = "Vehicle not found",
                    ErrorCode = ErrorsCodeConstants.VEHICLE_NOT_FOUND
                }, HttpStatusCode.NotFound
            );
        }

        var parkingHistoryOpen = await _parkingHistoryRepository.FindOneParkingHistoryOpen(
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

        var updatedParkingHistory = await _parkingHistoryRepository.UpdateTimeAndCostInParkingHistory(
                parkingLot.CostPerHour, 
                parkingHistoryOpen
            );

        vehicle.IsParked = false;

        await _vehicleRepository.UpdateVehicle(vehicle);

        parkingLot.FreeSpaces += 1;

        await _parkingLotRepository.UpdatedParkingLot(parkingLot);

        return updatedParkingHistory;
    }
}
