using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using System.ComponentModel;
using Vts.Api.Data;
using Vts.Api.Enums;
using Vts.Api.Factories;
using Vts.Api.Models;
using Vts.Api.Services;
using Vts.Api.Tools;

namespace Vts.Api.Tests.Services
{
    internal class InverseSolverServiceTests
    {
        private InverseSolverService _inverseSolverService;
        private ILoggerFactory _factory;
        private ILogger<InverseSolverService> _logger;
        private IPlotFactory _plotFactoryMock;
        private IParameterTools _parameterTools;
        [OneTimeSetUp]
        public void One_time_setup()
        {
            _plotFactoryMock = Substitute.For<IPlotFactory>();
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            _factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = _factory.CreateLogger<InverseSolverService>();
            _parameterTools = new ParameterTools();
            _inverseSolverService = new InverseSolverService(_logger, _plotFactoryMock, _parameterTools);
        }

        [Test]
        public void Test_get_plot_data()
        {
            const string postData = "{\"forwardSolverType\":\"PointSourceSDA\",\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRho\",\"measuredData\":[],\"xAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":19}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            _plotFactoryMock.GetPlot(PlotType.SolutionDomain, solutionDomainPlotParameters).Returns(new Plots());
            var results = _inverseSolverService.GetPlotData(solutionDomainPlotParameters);
            Assert.That(results, Is.InstanceOf<Plots>());
            _plotFactoryMock.Received(1).GetPlot(PlotType.SolutionDomain, solutionDomainPlotParameters);
        }

        [Test]
        public void Test_get_parameters_in_order_rofrho()
        {
            const string postData = "{\"forwardSolverType\":\"PointSourceSDA\",\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRho\",\"measuredData\":[],\"independentAxis\":{\"axis\":\"time\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":36}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(_parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties, solutionDomainPlotParameters.WavelengthOpticalPropertyList), solutionDomainPlotParameters.SolutionDomain, solutionDomainPlotParameters.XAxis, solutionDomainPlotParameters.IndependentAxis, solutionDomainPlotParameters.SecondIndependentAxis);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.That(test, Is.Not.Null);
            Assert.That(test[0].Mua, Is.EqualTo(0.01));
        }

        [Test]
        public void Test_get_parameters_in_order_roffx()
        {
            const string postData = "{\"forwardSolverType\":\"PointSourceSDA\",\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfFx\",\"measuredData\":[],\"xAxis\":{\"axis\":\"fx\",\"axisRange\": {\"start\":0,\"stop\":0.5,\"count\":\"51\"}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(_parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties, solutionDomainPlotParameters.WavelengthOpticalPropertyList), solutionDomainPlotParameters.SolutionDomain, solutionDomainPlotParameters.XAxis, solutionDomainPlotParameters.IndependentAxis, solutionDomainPlotParameters.SecondIndependentAxis);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.That(test, Is.Not.Null);
            Assert.That(test[0].Mua, Is.EqualTo(0.01));
        }

        [Test]
        public void Test_get_parameters_in_order_rofrhoandtime()
        {
            const string postData = "{\"forwardSolverType\":\"PointSourceSDA\",\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRhoAndTime\",\"measuredData\":[],\"independentAxis\":{\"axis\":\"time\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":36}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(_parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties, solutionDomainPlotParameters.WavelengthOpticalPropertyList), solutionDomainPlotParameters.SolutionDomain, solutionDomainPlotParameters.XAxis, solutionDomainPlotParameters.IndependentAxis, solutionDomainPlotParameters.SecondIndependentAxis);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.That(test, Is.Not.Null);
            Assert.That(test[0].Mua, Is.EqualTo(0.01));
        }

        [Test]
        public void Test_get_parameters_in_order_rofrhoandft()
        {
            const string postData = "{\"forwardSolverType\":\"PointSourceSDA\",\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRhoAndFt\",\"measuredData\":[],\"independentAxis\":{\"axis\":\"ft\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"rho\",\"axisRange\": {\"start\":0.5,\"stop\":9.5,\"count\":\"19\"}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(_parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties, solutionDomainPlotParameters.WavelengthOpticalPropertyList), solutionDomainPlotParameters.SolutionDomain, solutionDomainPlotParameters.XAxis, solutionDomainPlotParameters.IndependentAxis, solutionDomainPlotParameters.SecondIndependentAxis);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.That(test, Is.Not.Null);
            Assert.That(test[0].Mua, Is.EqualTo(0.01));
        }

        [Test]
        public void Test_get_parameters_in_order_roffxandtime()
        {
            const string postData = "{\"forwardSolverType\":\"PointSourceSDA\",\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfFxAndTime\",\"measuredData\":[],\"independentAxis\":{\"axis\":\"time\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"fx\",\"axisRange\": {\"start\":0,\"stop\":0.5,\"count\":\"51\"}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(_parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties, solutionDomainPlotParameters.WavelengthOpticalPropertyList), solutionDomainPlotParameters.SolutionDomain, solutionDomainPlotParameters.XAxis, solutionDomainPlotParameters.IndependentAxis, solutionDomainPlotParameters.SecondIndependentAxis);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.That(test, Is.Not.Null);
            Assert.That(test[0].Mua, Is.EqualTo(0.01));
        }

        [Test]
        public void Test_get_parameters_in_order_roffxandft()
        {
            const string postData = "{\"forwardSolverType\":\"PointSourceSDA\",\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfFxAndFt\",\"measuredData\":[],\"independentAxis\":{\"axis\":\"fx\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"fx\",\"axisRange\": {\"start\":0,\"stop\":0.5,\"count\":\"51\"}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var initialGuessParams = _parameterTools.GetParametersInOrder(_parameterTools.GetOpticalPropertiesObject(solutionDomainPlotParameters.OpticalProperties, solutionDomainPlotParameters.WavelengthOpticalPropertyList), solutionDomainPlotParameters.SolutionDomain, solutionDomainPlotParameters.XAxis, solutionDomainPlotParameters.IndependentAxis, solutionDomainPlotParameters.SecondIndependentAxis);
            var test = (OpticalProperties[])initialGuessParams[IndependentVariableAxis.Wavelength];
            Assert.That(test, Is.Not.Null);
            Assert.That(test[0].Mua, Is.EqualTo(0.01));
        }

        [Test]
        public void GetParametersInOrder_throws_exception()
        {
            Assert.Throws<InvalidEnumArgumentException>(() => _parameterTools.GetParametersInOrder(null, (SolutionDomainType)99, new IndependentAxis(), new IndependentAxis(), new IndependentAxis()));
        }

        [Test]
        public void Test_GetParameterOrder_throws_exception()
        {
            Assert.Throws<InvalidEnumArgumentException>(() => ParameterTools.GetParameterOrder((IndependentVariableAxis)99));
        }

        [Test]
        public void Test_GetParameterOrder_returns_integer()
        {
            Assert.That(ParameterTools.GetParameterOrder(IndependentVariableAxis.Wavelength), Is.EqualTo(0));
            Assert.That(ParameterTools.GetParameterOrder(IndependentVariableAxis.Rho), Is.EqualTo(1));
            Assert.That(ParameterTools.GetParameterOrder(IndependentVariableAxis.Fx), Is.EqualTo(1));
            Assert.That(ParameterTools.GetParameterOrder(IndependentVariableAxis.Time), Is.EqualTo(2));
            Assert.That(ParameterTools.GetParameterOrder(IndependentVariableAxis.Ft), Is.EqualTo(2));
            Assert.That(ParameterTools.GetParameterOrder(IndependentVariableAxis.Z), Is.EqualTo(3));
        }

        [Test]
        public void Test_get_plot_data_throws_error()
        {
            Assert.Throws<NullReferenceException>(() => _inverseSolverService.GetPlotData(null));
        }
    }
}