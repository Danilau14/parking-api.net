using AutoMapper;
using ParkingApi.Dto.ParkingsLot;
using ParkingApi.Models;

namespace ParkingApi.Mappings.ParkingsLot;

public class UpdateParkingLotMapping : Profile
{
    public UpdateParkingLotMapping()
    {
        CreateMap<UpdatedParkingLotDto, ParkingLot>()
                .ForMember(dest => dest.Size, opt => opt.Condition(src => src.Size.HasValue))  
                .ForMember(dest => dest.CostPerHour, opt => opt.Condition(src => src.CostPerHour.HasValue)) 
                .ForMember(dest => dest.UserId, opt => opt.Condition(src => src.PartnerId.HasValue));
    }
}
