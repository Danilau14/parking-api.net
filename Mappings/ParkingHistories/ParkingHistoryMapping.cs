using AutoMapper;
using ParkingApi.Dto.ParkingHistory;
using ParkingApi.Models;



namespace ParkingApi.Mappings.ParkingHistories;

public class ParkingHistoryMapping : Profile
{
    public ParkingHistoryMapping()
    {
        CreateMap<ParkingHistory, ParkingHistoryDto>();
    }
}
