using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using Vts.Api.Security;

namespace Vts.Api.Test.Security
{
    class ApiKeyRequirementHandlerTests
    {
        [Test]
        public async Task Test_api_key_requirement_handler_succeed()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("X-API-KEY", "TESTAPIKEY");
            var requirements = new [] { new ApiKeyRequirement(new[] { "TESTAPIKEY" })};
            // we need to create a shell of the AuthorizationHandlerContext, that contains the header information 
            var actionContext = new ActionContext(context, new RouteData(), new ActionDescriptor());
            // we need to include the ApiControllerAttribute here to mimic the call to the API
            var filters = new List<IFilterMetadata>() {
                new ApiControllerAttribute(),
            };
            var authFilterContext = new AuthorizationFilterContext(actionContext, filters);
            var authHandlerContext = new AuthorizationHandlerContext(requirements, ClaimsPrincipal.Current, authFilterContext);
            var keyService = new ApiKeyRequirementHandler();
            await keyService.HandleAsync(authHandlerContext);
            Assert.IsTrue(authHandlerContext.HasSucceeded);
        }

        [Test]
        public async Task Test_api_key_requirement_handler_fail()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("X-API-KEY", "INVALIDKEY");
            var requirements = new[] { new ApiKeyRequirement(new[] { "TESTAPIKEY" }) };
            var actionContext = new ActionContext(context, new RouteData(), new ActionDescriptor());
            var filters = new List<IFilterMetadata>() {
                new ApiControllerAttribute(),
            };
            var authFilterContext = new AuthorizationFilterContext(actionContext, filters);
            var authHandlerContext = new AuthorizationHandlerContext(requirements, ClaimsPrincipal.Current, authFilterContext);
            var keyService = new ApiKeyRequirementHandler();
            await keyService.HandleAsync(authHandlerContext);
            Assert.IsTrue(authHandlerContext.HasFailed);
        }
    }
}
