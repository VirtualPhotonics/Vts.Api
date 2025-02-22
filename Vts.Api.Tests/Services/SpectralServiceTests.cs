using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using Vts.Api.Data;
using Vts.Api.Enums;
using Vts.Api.Factories;
using Vts.Api.Models;
using Vts.Api.Services;
using Vts.Common;

namespace Vts.Api.Tests.Services
{
    internal class SpectralServiceTests
    {
        private SpectralService _spectralService;
        private ILoggerFactory _factory;
        private ILogger<SpectralService> _logger;
        private IPlotFactory _plotFactoryMock;
        [OneTimeSetUp]
        public void One_time_setup()
        {
            _plotFactoryMock = Substitute.For<IPlotFactory>();
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            _factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = _factory.CreateLogger<SpectralService>();
            _spectralService = new SpectralService(_logger, _plotFactoryMock);
        }

        [Test]
        public void Test_get_plot_data_powerlaw()
        {
            const string postData = "{\"spectralPlotType\":\"musp\",\"plotName\":\"μs'\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638},\"scatteringType\":\"PowerLaw\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42},\"intralipidScatterer\":{\"volumeFraction\":0.01},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01},\"xAxis\":{\"axis\":\"wavelength\",\"axisRange\":{\"start\":650,\"stop\":1000,\"count\":36}}}";
            var xAxis = new DoubleRange(650, 1000, 36);
            var wavelengths = xAxis.AsEnumerable().ToArray();
            var spectralPlotParameters = JsonConvert.DeserializeObject<SpectralPlotParameters>(postData);
            _plotFactoryMock.GetPlot(PlotType.Spectral, spectralPlotParameters).Returns(new Plots());
            var results = _spectralService.GetPlotData(spectralPlotParameters);
            Assert.That(results, Is.InstanceOf<Plots>());
            _plotFactoryMock.Received(1).GetPlot(PlotType.Spectral, spectralPlotParameters);
            Assert.That(spectralPlotParameters, Is.Not.Null);
            Assert.That(spectralPlotParameters.Wavelengths[4], Is.EqualTo(wavelengths[4]));
            Assert.That(spectralPlotParameters.Tissue.TissueType, Is.EqualTo(TissueType.Skin));
        }

        [Test]
        public void Test_get_plot_data_powerlaw_default()
        {
            const string postData = "{\"spectralPlotType\":\"musp\",\"plotName\":\"μs'\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638},\"scatteringType\":\"PowerLaw\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42},\"intralipidScatterer\":{\"volumeFraction\":0.01},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01},\"xAxis\":{\"axis\":\"wavelength\",\"axisRange\":{\"start\":650,\"stop\":1000,\"count\":36}}}";
            var xAxis = new DoubleRange(650, 1000, 36);
            var wavelengths = xAxis.AsEnumerable().ToArray();
            var spectralPlotParameters = JsonConvert.DeserializeObject<SpectralPlotParameters>(postData);
            Assert.That(spectralPlotParameters, Is.Not.Null);
            spectralPlotParameters.ScatteringType = (ScatteringType)99;
            _plotFactoryMock.GetPlot(PlotType.Spectral, spectralPlotParameters).Returns(new Plots());
            var results = _spectralService.GetPlotData(spectralPlotParameters);
            Assert.That(results, Is.InstanceOf<Plots>());
            _plotFactoryMock.Received(1).GetPlot(PlotType.Spectral, spectralPlotParameters);
            Assert.That(spectralPlotParameters.Wavelengths[4], Is.EqualTo(wavelengths[4]));
            Assert.That(spectralPlotParameters.Tissue.TissueType, Is.EqualTo(TissueType.Skin));
        }

        [Test]
        public void Test_get_plot_data_mie()
        {
            const string postData = "{\"spectralPlotType\":\"musp\",\"plotName\":\"μs'\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638},\"scatteringType\":\"Mie\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42},\"intralipidScatterer\":{\"volumeFraction\":0.01},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01},\"xAxis\":{\"axis\":\"wavelength\",\"axisRange\":{\"start\":650,\"stop\":1000,\"count\":36}}}";
            var spectralPlotParameters = JsonConvert.DeserializeObject<SpectralPlotParameters>(postData);
            _plotFactoryMock.GetPlot(PlotType.Spectral, spectralPlotParameters).Returns(new Plots());
            var results = _spectralService.GetPlotData(spectralPlotParameters);
            Assert.That(results, Is.InstanceOf<Plots>());
            _plotFactoryMock.Received(1).GetPlot(PlotType.Spectral, spectralPlotParameters);
        }

        [Test]
        public void Test_get_plot_data_intralipid()
        {
            const string postData = "{\"spectralPlotType\":\"musp\",\"plotName\":\"μs'\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638},\"scatteringType\":\"Intralipid\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42},\"intralipidScatterer\":{\"volumeFraction\":0.01},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01},\"xAxis\":{\"axis\":\"wavelength\",\"axisRange\":{\"start\":650,\"stop\":1000,\"count\":36}}}";
            var spectralPlotParameters = JsonConvert.DeserializeObject<SpectralPlotParameters>(postData);
            _plotFactoryMock.GetPlot(PlotType.Spectral, spectralPlotParameters).Returns(new Plots());
            var results = _spectralService.GetPlotData(spectralPlotParameters);
            Assert.That(results, Is.InstanceOf<Plots>());
            _plotFactoryMock.Received(1).GetPlot(PlotType.Spectral, spectralPlotParameters);
        }
    }
}