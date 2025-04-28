namespace ParkingApi.Mappings.ParkingHistories;

public class ParkingHistoryMapping : Profile
{
    public ParkingHistoryMapping()
    {
        CreateMap<ParkingHistory, ParkingHistoryDto>();
    }
}
