namespace ParkingApi.Application.Features.ParkingLots.Dtos;

public class ParkingLotMapping : Profile
{
    public ParkingLotMapping()
    {
        CreateMap<ParkingLot, ParkingLotDto>()
            .ForMember(dest => dest.PartnerId, opt => opt.MapFrom(src => src.UserId));
    }
}
