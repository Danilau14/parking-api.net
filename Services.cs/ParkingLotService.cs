namespace ParkingApi.Services.cs;

public class ParkingLotService : IParkingLotService
{
    private readonly IParkingLotRepository _parkingLotRepository;
    private readonly IUserRepository _userRepository;

    public ParkingLotService(
        IParkingLotRepository parkingLotRepository,
        IUserRepository userRepository
        )
    {
        _parkingLotRepository = parkingLotRepository;
        _userRepository = userRepository;
    }

    public async Task<ParkingLot> CreateParkigLotAsync(ParkingLot parkingLot)
    {
        if (null != parkingLot.UserId)
        {
            var partnerExist = await _userRepository.GetByIdAsync(parkingLot.UserId.Value);
            if (null == partnerExist)
            {
                throw new EipexException(new ErrorResponse
                {
                    Message = "Partner not found",
                    ErrorCode = ErrorsCodeConstants.PARTNER_INVALID
                }, HttpStatusCode.NotFound
);
            }
        }

        await _parkingLotRepository.CreateParkingLot(parkingLot);

        return parkingLot;
    }
}
