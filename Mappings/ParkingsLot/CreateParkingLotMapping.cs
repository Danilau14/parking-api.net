using ParkingApi.Application.Features.ParkingLots.Dtos;

namespace ParkingApi.Mappings.ParkingsLot;

public class CreateParkingLotMapping : Profile
{
    public CreateParkingLotMapping()
    {
        CreateMap<CreateParkingLotDto, ParkingLot>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.PartnerId))
            .ForMember(dest => dest.FreeSpaces, opt => opt.MapFrom(src => src.Size));
    }
}
