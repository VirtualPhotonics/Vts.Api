using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Vts.Api.Services;
using Vts.Api.Tools;

namespace Vts.Api.Tests
{
    internal class StartupTests
    {
        private IWebHost? _host;

        [Test]
        public void Test_startup()
        {
            // setup a test configuration to test the startup
            var testConfiguration = new Dictionary<string, string>();
            _host = WebHost.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    var replacementConfiguration = new ConfigurationBuilder()
                        .AddInMemoryCollection(testConfiguration)
                        .Build();

                    config.AddConfiguration(replacementConfiguration);
                })
                .UseStartup<Startup>()
                .Build();
            // Check the services were added correctly
            Assert.IsNotNull(_host.Services.GetService<IForwardSolverService>());
            Assert.IsNotNull(_host.Services.GetService<IInverseSolverService>());
            Assert.IsNotNull(_host.Services.GetService<IAuthorizationHandler>());
            Assert.IsNotNull(_host.Services.GetService<IParameterTools>());
            Assert.IsNotNull(_host.Services.GetService<PlotSolutionDomainResultsService>());
            Assert.IsNotNull(_host.Services.GetService<PlotSpectralResultsService>());
        }
    }
}
