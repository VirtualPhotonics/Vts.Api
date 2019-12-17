using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Vts.Api.Enums;
using Vts.Api.Factories;
using Vts.Api.Models;
using Vts.Api.Services;
using Vts.Api.Tools;

namespace Vts.Api.Tests.Services
{
    class InverseSolverServiceTests
    {
        private InverseSolverService _inverseSolverService;
        private ILoggerFactory _factory;
        private ILogger<InverseSolverService> _logger;
        private Mock<IPlotFactory> _plotFactoryMock;
        private IParameterTools _parameterTools;

        [OneTimeSetUp]
        public void One_time_setup()
        {
            _plotFactoryMock = new Mock<IPlotFactory>();
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            _factory = serviceProvider.GetService<ILoggerFactory>()
                .AddConsole();
            _logger = _factory.CreateLogger<InverseSolverService>();
            _parameterTools = new ParameterTools();
            _inverseSolverService = new InverseSolverService(_logger, _plotFactoryMock.Object, _parameterTools);
        }

        [Test]
        public void Test_get_plot_data()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRho\",\"measuredData\":[],\"independentAxes\":{\"label\":\"t\",\"value\":0.05},\"xAxis\":{\"start\":0.5,\"stop\":9.5,\"count\":\"36\"},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            _plotFactoryMock.Setup(x => x.GetPlot(PlotType.SolutionDomain, solutionDomainPlotParameters)).Returns("");
            var results = _inverseSolverService.GetPlotData(solutionDomainPlotParameters);
            Assert.AreEqual("", results);
            _plotFactoryMock.Verify(mock => mock.GetPlot(PlotType.SolutionDomain, solutionDomainPlotParameters), Times.Once);
        }

        [Test]
        public void Test_get_parameters_in_order_rofrho()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRho\",\"measuredData\":[],\"independentAxes\":{\"label\":\"t\",\"value\":0.05},\"xAxis\":{\"start\":0.5,\"stop\":9.5,\"count\":\"36\"},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(
                _parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties),
                solutionDomainPlotParameters.XAxis.AsEnumerable().ToArray(),
                solutionDomainPlotParameters.SolutionDomain,
                solutionDomainPlotParameters.IndependentAxes.Label,
                solutionDomainPlotParameters.IndependentAxes.Value);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.IsNotNull(test);
            Assert.AreEqual(0.01, test[0].Mua);
        }

        [Test]
        public void Test_get_parameters_in_order_roffx()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfFx\",\"measuredData\":[],\"independentAxes\":{\"label\":\"t\",\"value\":0.05},\"xAxis\":{\"start\":0,\"stop\":0.5,\"count\":\"51\"},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(
                _parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties),
                solutionDomainPlotParameters.XAxis.AsEnumerable().ToArray(),
                solutionDomainPlotParameters.SolutionDomain,
                solutionDomainPlotParameters.IndependentAxes.Label,
                solutionDomainPlotParameters.IndependentAxes.Value);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.IsNotNull(test);
            Assert.AreEqual(0.01, test[0].Mua);
        }

        [Test]
        public void Test_get_parameters_in_order_rofrhoandtime()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRhoAndTime\",\"measuredData\":[],\"independentAxes\":{\"label\":\"t\",\"value\":0.05},\"xAxis\":{\"start\":0.5,\"stop\":9.5,\"count\":\"36\"},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(
                _parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties),
                solutionDomainPlotParameters.XAxis.AsEnumerable().ToArray(),
                solutionDomainPlotParameters.SolutionDomain,
                solutionDomainPlotParameters.IndependentAxes.Label,
                solutionDomainPlotParameters.IndependentAxes.Value);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.IsNotNull(test);
            Assert.AreEqual(0.01, test[0].Mua);
        }

        [Test]
        public void Test_get_parameters_in_order_rofrhoandft()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRhoAndFt\",\"measuredData\":[],\"independentAxes\":{\"label\":\"ft\",\"value\":0.05},\"xAxis\":{\"start\":0.5,\"stop\":9.5,\"count\":\"19\"},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(
                _parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties),
                solutionDomainPlotParameters.XAxis.AsEnumerable().ToArray(),
                solutionDomainPlotParameters.SolutionDomain,
                solutionDomainPlotParameters.IndependentAxes.Label,
                solutionDomainPlotParameters.IndependentAxes.Value);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.IsNotNull(test);
            Assert.AreEqual(0.01, test[0].Mua);
        }

        [Test]
        public void Test_get_parameters_in_order_roffxandtime()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfFxAndTime\",\"measuredData\":[],\"independentAxes\":{\"label\":\"t\",\"value\":0.05},\"xAxis\":{\"start\":0,\"stop\":0.5,\"count\":\"51\"},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(
                _parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties),
                solutionDomainPlotParameters.XAxis.AsEnumerable().ToArray(),
                solutionDomainPlotParameters.SolutionDomain,
                solutionDomainPlotParameters.IndependentAxes.Label,
                solutionDomainPlotParameters.IndependentAxes.Value);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.IsNotNull(test);
            Assert.AreEqual(0.01, test[0].Mua);
        }

        [Test]
        public void Test_get_parameters_in_order_roffxandft()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfFxAndFt\",\"measuredData\":[],\"independentAxes\":{\"label\":\"fx\",\"value\":0.05},\"xAxis\":{\"start\":0,\"stop\":0.5,\"count\":\"51\"},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(
                _parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties),
                solutionDomainPlotParameters.XAxis.AsEnumerable().ToArray(),
                solutionDomainPlotParameters.SolutionDomain,
                solutionDomainPlotParameters.IndependentAxes.Label,
                solutionDomainPlotParameters.IndependentAxes.Value);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.IsNotNull(test);
            Assert.AreEqual(0.01, test[0].Mua);
        }
    }
}
