using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using Vts.Api.Data;
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
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRho\",\"measuredData\":[],\"xAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":19}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            _plotFactoryMock.Setup(x => x.GetPlot(PlotType.SolutionDomain, solutionDomainPlotParameters)).Returns(new Plots());
            var results = _inverseSolverService.GetPlotData(solutionDomainPlotParameters);
            Assert.IsInstanceOf<Plots>(results);
            _plotFactoryMock.Verify(mock => mock.GetPlot(PlotType.SolutionDomain, solutionDomainPlotParameters), Times.Once);
        }

        [Test]
        public void Test_get_parameters_in_order_rofrho()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRho\",\"measuredData\":[],\"independentAxis\":{\"axis\":\"time\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":36}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(
                _parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties, solutionDomainPlotParameters.WavelengthOpticalPropertyList),
                solutionDomainPlotParameters.SolutionDomain,
                solutionDomainPlotParameters.XAxis,
                solutionDomainPlotParameters.IndependentAxis,
                solutionDomainPlotParameters.SecondIndependentAxis);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.IsNotNull(test);
            Assert.AreEqual(0.01, test[0].Mua);
        }

        [Test]
        public void Test_get_parameters_in_order_roffx()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfFx\",\"measuredData\":[],\"xAxis\":{\"axis\":\"fx\",\"axisRange\": {\"start\":0,\"stop\":0.5,\"count\":\"51\"}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(
                _parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties, solutionDomainPlotParameters.WavelengthOpticalPropertyList),
                solutionDomainPlotParameters.SolutionDomain,
                solutionDomainPlotParameters.XAxis,
                solutionDomainPlotParameters.IndependentAxis,
                solutionDomainPlotParameters.SecondIndependentAxis);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.IsNotNull(test);
            Assert.AreEqual(0.01, test[0].Mua);
        }

        [Test]
        public void Test_get_parameters_in_order_rofrhoandtime()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRhoAndTime\",\"measuredData\":[],\"independentAxis\":{\"axis\":\"time\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":36}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(
                _parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties, solutionDomainPlotParameters.WavelengthOpticalPropertyList),
                solutionDomainPlotParameters.SolutionDomain,
                solutionDomainPlotParameters.XAxis,
                solutionDomainPlotParameters.IndependentAxis,
                solutionDomainPlotParameters.SecondIndependentAxis);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.IsNotNull(test);
            Assert.AreEqual(0.01, test[0].Mua);
        }

        [Test]
        public void Test_get_parameters_in_order_rofrhoandft()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRhoAndFt\",\"measuredData\":[],\"independentAxis\":{\"axis\":\"ft\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"rho\",\"axisRange\": {\"start\":0.5,\"stop\":9.5,\"count\":\"19\"}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(
                _parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties, solutionDomainPlotParameters.WavelengthOpticalPropertyList),
                solutionDomainPlotParameters.SolutionDomain,
                solutionDomainPlotParameters.XAxis,
                solutionDomainPlotParameters.IndependentAxis,
                solutionDomainPlotParameters.SecondIndependentAxis);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.IsNotNull(test);
            Assert.AreEqual(0.01, test[0].Mua);
        }

        [Test]
        public void Test_get_parameters_in_order_roffxandtime()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfFxAndTime\",\"measuredData\":[],\"independentAxis\":{\"axis\":\"time\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"fx\",\"axisRange\": {\"start\":0,\"stop\":0.5,\"count\":\"51\"}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(
                _parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties, solutionDomainPlotParameters.WavelengthOpticalPropertyList),
                solutionDomainPlotParameters.SolutionDomain,
                solutionDomainPlotParameters.XAxis,
                solutionDomainPlotParameters.IndependentAxis,
                solutionDomainPlotParameters.SecondIndependentAxis);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.IsNotNull(test);
            Assert.AreEqual(0.01, test[0].Mua);
        }

        [Test]
        public void Test_get_parameters_in_order_roffxandft()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfFxAndFt\",\"measuredData\":[],\"independentAxis\":{\"axis\":\"fx\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"fx\",\"axisRange\": {\"start\":0,\"stop\":0.5,\"count\":\"51\"}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(
                _parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties, solutionDomainPlotParameters.WavelengthOpticalPropertyList),
                solutionDomainPlotParameters.SolutionDomain,
                solutionDomainPlotParameters.XAxis,
                solutionDomainPlotParameters.IndependentAxis,
                solutionDomainPlotParameters.SecondIndependentAxis);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.IsNotNull(test);
            Assert.AreEqual(0.01, test[0].Mua);
        }

        [Test]
        public void Test_get_plot_data_throws_error()
        {
            Assert.Throws<NullReferenceException>(() =>
                    _inverseSolverService.GetPlotData(null));
        }
    }
}
