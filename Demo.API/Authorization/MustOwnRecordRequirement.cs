using Microsoft.AspNetCore.Authorization;

namespace Demo.API.Authorization
{
    public class MustOwnRecordRequirement : IAuthorizationRequirement
    {
    }
}
