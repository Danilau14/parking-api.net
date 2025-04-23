using AutoMapper;
using ParkingApi.Dto;
using ParkingApi.Models;

namespace ParkingApi.Mappings;

public class CreateUserMapping : Profile
{
    public CreateUserMapping()
    {
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.ParkingLots, opt => opt.MapFrom(src => new List<ParkingLot>()));
    }
}
