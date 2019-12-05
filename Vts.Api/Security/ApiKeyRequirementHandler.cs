using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Vts.Api.Security
{
    public class ApiKeyRequirementHandler : AuthorizationHandler<ApiKeyRequirement>
    {
        public const string API_KEY_HEADER_NAME = "X-API-KEY";

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            SucceedRequirementIfApiKeyPresentAndValid(context, requirement);
            return Task.CompletedTask;
        }

        private void SucceedRequirementIfApiKeyPresentAndValid(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
        {
            if (context.Resource is AuthorizationFilterContext authorizationFilterContext)
            {
                var apiKey = authorizationFilterContext.HttpContext.Request.Headers[API_KEY_HEADER_NAME].FirstOrDefault();
                if (apiKey != null && requirement.ApiKeys.Any(requiredApiKey => apiKey == requiredApiKey))
                {
                    context.Succeed(requirement);
                } else
                {
                    authorizationFilterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Fail();
                }
            }
        }
    }
}
