using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Collections.Generic;
using Vts.Api.Services;
using Vts.Api.Tools;

namespace Vts.Api.Tests
{
    internal class StartupTests
    {
        private ILogger<Startup> _logger;
        private ILoggerFactory _factory;

        [SetUp]
        public void Setup_data()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            _factory = serviceProvider.GetService<ILoggerFactory>()
                .AddConsole();
            _logger = _factory.CreateLogger<Startup>();
        }

        [Test]
        public void Test_startup()
        {
            // setup a test configuration to test the strartup
            var testConfiguration = new Dictionary<string, string>();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(testConfiguration)
                .Build();
            // Call the Startup constructor with the generated configuration
            var target = new Startup(configuration, _logger);
            var services = new ServiceCollection()
                .AddLogging();
            // Call ConfigureServices
            target.ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            // Check the services were added correctly
            Assert.IsNotNull(serviceProvider.GetService<IForwardSolverService>());
            Assert.IsNotNull(serviceProvider.GetService<IInverseSolverService>());
            Assert.IsNotNull(serviceProvider.GetService<IAuthorizationHandler>());
            Assert.IsNotNull(serviceProvider.GetService<IParameterTools>());
            Assert.IsNotNull(serviceProvider.GetService<PlotSolutionDomainResultsService>());
            Assert.IsNotNull(serviceProvider.GetService<PlotSpectralResultsService>());
        }
    }
}
