namespace ParkingApi.Application.Mappings.ParkingsLot;

public class ParkingLotMapping : Profile
{
    public ParkingLotMapping()
    {
        CreateMap<ParkingLot, ParkingLotDto>()
            .ForMember(dest => dest.PartnerId, opt => opt.MapFrom(src => src.UserId));
    }
}
