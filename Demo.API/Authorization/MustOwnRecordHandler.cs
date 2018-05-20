using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Authorization
{
    public class MustOwnRecordHandler : AuthorizationHandler<MustOwnRecordRequirement>
    {

        public MustOwnRecordHandler()
        {
            // TODO - inject repositroy here
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustOwnRecordRequirement requirement)
        {
            var filtereContext = context?.Resource as AuthorizationFilterContext;
            var productId = filtereContext?.RouteData?.Values["id"]?.ToString();
            var ownerId = context?.User?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (context == null || productId == null || ownerId == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            if (ownerId == ownerId)  // TODO - fetch record from database
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            else
            {
                context.Fail();
                return Task.CompletedTask;
            }


        }
    }
}
