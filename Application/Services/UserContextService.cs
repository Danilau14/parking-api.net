using ParkingApi.Core.Interfaces;

namespace ParkingApi.Application.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? GetCurrentUserId()
    {
        var userId =  _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null) return null;

        return int.Parse(userId);
    }

    public string? GetCurrentEmail()
    {
        var email = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        if (email == null) return null;

        return email;
    }
}
