using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            Assert.That(_host.Services.GetService<IForwardSolverService>(), Is.Not.Null);
            Assert.That(_host.Services.GetService<IInverseSolverService>(), Is.Not.Null);
            Assert.That(_host.Services.GetService<IAuthorizationHandler>(), Is.Not.Null);
            Assert.That(_host.Services.GetService<IParameterTools>(), Is.Not.Null);
            Assert.That(_host.Services.GetService<PlotSolutionDomainResultsService>(), Is.Not.Null);
            Assert.That(_host.Services.GetService<PlotSpectralResultsService>(), Is.Not.Null);
        }
    }
}
