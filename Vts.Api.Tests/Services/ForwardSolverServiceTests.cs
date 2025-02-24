using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Vts.Api.Data;
using Vts.Api.Enums;
using Vts.Api.Factories;
using Vts.Api.Models;
using Vts.Api.Services;

namespace Vts.Api.Tests.Services
{
    internal class ForwardSolverServiceTests
    {
        private ForwardSolverService? _forwardSolverService;
        private ILoggerFactory? _factory;
        private ILogger<ForwardSolverService>? _logger;
        private IPlotFactory? _plotFactoryMock;
        [Test]
        public void Test_get_plot_data()
        {
            _plotFactoryMock = Substitute.For<IPlotFactory>();
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            _factory = serviceProvider.GetService<ILoggerFactory>();
            Assert.That(_factory, Is.Not.Null);
            if (_factory != null) _logger = _factory.CreateLogger<ForwardSolverService>();
            _forwardSolverService = new ForwardSolverService(_logger, _plotFactoryMock);
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRho\",\"independentAxes\":{\"label\":\"t\",\"value\":0.05},\"xAxis\":{\"start\":0.5,\"stop\":9.5,\"count\":19},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            _plotFactoryMock.GetPlot(PlotType.SolutionDomain, solutionDomainPlotParameters).Returns(new Plots());
            var results = _forwardSolverService.GetPlotData(solutionDomainPlotParameters);
            Assert.That(results, Is.InstanceOf<Plots>());
            _plotFactoryMock.Received(1).GetPlot(PlotType.SolutionDomain, solutionDomainPlotParameters);
        }

        [Test]
        public void Test_get_plot_data_throws_error()
        {
            _plotFactoryMock = Substitute.For<IPlotFactory>();
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            _factory = serviceProvider.GetService<ILoggerFactory>();
            Assert.That(_factory, Is.Not.Null);
            if (_factory != null) _logger = _factory.CreateLogger<ForwardSolverService>();
            _forwardSolverService = new ForwardSolverService(_logger, _plotFactoryMock);
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRho\",\"xAxis\":{\"start\":0.5,\"stop\":9.5,\"count\":19},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            _plotFactoryMock.GetPlot(PlotType.SolutionDomain, solutionDomainPlotParameters).Throws<ArgumentNullException>();
            Assert.Throws<ArgumentNullException>(() => _forwardSolverService.GetPlotData(solutionDomainPlotParameters));
            _plotFactoryMock.Received(1).GetPlot(PlotType.SolutionDomain, solutionDomainPlotParameters);
        }
    }
}