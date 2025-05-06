namespace ParkingApi.Application.Mappings.Users;

public class CreateUserMapping : Profile
{
    public CreateUserMapping()
    {
        CreateMap<CreateUserDto, User>();
    }
}
