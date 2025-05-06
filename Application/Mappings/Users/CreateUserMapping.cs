namespace ParkingApi.Application.Mappings.Users;

public class CreateUserMapping : Profile
{
    public CreateUserMapping()
    {
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.ParkingLots, opt => opt.MapFrom(src => new List<ParkingLot>()));
    }
}
