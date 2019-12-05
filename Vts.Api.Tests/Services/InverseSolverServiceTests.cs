using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Vts.Api.Services;

namespace Vts.Api.Test.Services
{
    class InverseSolverServiceTests
    {
        private InverseSolverService inverseSolverService;
        private ILoggerFactory factory;
        private ILogger<InverseSolverService> logger;

        [Test]
        public void Test_get_plot_data()
        {
            //var serviceProvider = new ServiceCollection()
            //    .AddLogging()
            //    .BuildServiceProvider();
            //factory = serviceProvider.GetService<ILoggerFactory>()
            //    .AddConsole();
            //logger = factory.CreateLogger<InverseSolverService>();
            //inverseSolverService = new InverseSolverService();
            //    // solutionDomain: SolutionDomain = { value: 'ROfRho' };
            //string results = inverseSolverService.GetPlotData("{"solutionDomain"='ROfRho'}");
            //Assert.AreEqual("", results);
        }
    }
}
