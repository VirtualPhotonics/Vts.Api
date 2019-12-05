using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Vts.Api.Enums;
using Vts.Api.Factories;
using Vts.Api.Models;
using Vts.Api.Services;

namespace Vts.Api.Tests.Services
{
    class ForwardSolverServiceTests
    {
        private ForwardSolverService _forwardSolverService;
        private ILoggerFactory _factory;
        private ILogger<ForwardSolverService> _logger;
        private Mock<IPlotFactory> _plotFactoryMock;

        [Test]
        public void Test_get_plot_data()
        {
            _plotFactoryMock = new Mock<IPlotFactory>();
            _plotFactoryMock.Setup(x => x.GetPlot(PlotType.SolutionDomain, new SolutionDomainPlotParameters())).Returns("");
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            _factory = serviceProvider.GetService<ILoggerFactory>()
                .AddConsole();
            _logger = _factory.CreateLogger<ForwardSolverService>();
            _forwardSolverService = new ForwardSolverService(_logger, _plotFactoryMock.Object);
            var postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRho\",\"independentAxes\":{\"show\":false,\"first\":\"ρ\",\"second\":\"t\",\"label\":\"t\",\"value\":0.05,\"units\":\"ns\",\"firstUnits\":\"mm\",\"secondUnits\":\"ns\"},\"xAxis\":{\"title\":\"Detector Positions\",\"startLabel\":\"Begin\",\"startLabelUnits\":\"mm\",\"start\":0.5,\"endLabel\":\"End\",\"endLabelUnits\":\"mm\",\"stop\":9.5,\"numberLabel\":\"Number\",\"count\":19},\"opticalProperties\":{\"title\":\"Optical Properties\",\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var results = _forwardSolverService.GetPlotData(JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData));
            Assert.IsNull(results);
        }
    }
}
