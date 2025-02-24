using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Vts.Api.Security;

namespace Vts.Api.Tests.Security
{
    internal class ApiKeyRequirementTests
    {
        [Test]
        public void Test_api_key_handler()
        {
            var serviceProvider = new ServiceCollection()
                .AddAuthorization(authConfig =>
                {
                    authConfig.AddPolicy("ApiKeyPolicy",
                        policyBuilder => policyBuilder
                        .AddRequirements(new ApiKeyRequirement(new[] { "TESTKEY" })));
                })
                    .BuildServiceProvider();
            var service = serviceProvider.GetService<IAuthorizationPolicyProvider>();
            var requirements = service.GetPolicyAsync("ApiKeyPolicy").Result.Requirements;
            var apiRequirement = (ApiKeyRequirement)requirements[0];
            var key = apiRequirement.ApiKeys[0];
            Assert.That(key, Is.EqualTo("TESTKEY"));
        }
    }
}
