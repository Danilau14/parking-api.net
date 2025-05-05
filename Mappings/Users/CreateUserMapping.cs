using ParkingApi.Application.Features.Users.Dtos;
using ParkingApi.Core.Models;

namespace ParkingApi.Mappings.Users;

public class CreateUserMapping : Profile
{
    public CreateUserMapping()
    {
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.ParkingLots, opt => opt.MapFrom(src => new List<ParkingLot>()));
    }
}
