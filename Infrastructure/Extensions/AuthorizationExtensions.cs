using ParkingApi.Core.Enums;

namespace ParkingApi.Infrastructure.Extensions;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PoliciesConstants.ADMIN_POLICY, policy =>
                policy.RequireClaim(ClaimTypes.Role, UserRole.ADMIN.ToString()));

            options.AddPolicy(PoliciesConstants.PARTNER_POLICY, policy =>
                policy.RequireClaim(ClaimTypes.Role, UserRole.PARTNER.ToString()));
        });

        return services;
    }
}
