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
using Vts.Common;

namespace Vts.Api.Tests.Services
{
    class SpectralServiceTests
    {
        private SpectralService _spectralService;
        private ILoggerFactory _factory;
        private ILogger<SpectralService> _logger;
        private Mock<IPlotFactory> _plotFactoryMock;

        [OneTimeSetUp]
        public void One_time_setup()
        {
            _plotFactoryMock = new Mock<IPlotFactory>();
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            _factory = serviceProvider.GetService<ILoggerFactory>()
                .AddConsole();
            _logger = _factory.CreateLogger<SpectralService>();
            _spectralService = new SpectralService(_logger, _plotFactoryMock.Object);
        }

        [Test]
        public void Test_get_plot_data_powerlaw()
        {
            var postData = "{\"spectralPlotType\":\"musp\",\"plotName\":\"μs'\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638},\"scatteringType\":\"PowerLaw\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42},\"intralipidScatterer\":{\"volumeFraction\":0.01},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01},\"xAxis\":{\"axis\":\"wavelength\",\"axisRange\":{\"start\":650,\"stop\":1000,\"count\":36}}}";
            var xAxis = new DoubleRange(650, 1000, 36);
            var wavelengths = xAxis.AsEnumerable().ToArray();
            var spectralPlotParameters = JsonConvert.DeserializeObject<SpectralPlotParameters>(postData);
            _plotFactoryMock.Setup(x => x.GetPlot(PlotType.Spectral, spectralPlotParameters)).Returns("");
            var results = _spectralService.GetPlotData(spectralPlotParameters);
            Assert.AreEqual("", results);
            _plotFactoryMock.Verify(mock => mock.GetPlot(PlotType.Spectral, spectralPlotParameters), Times.Once);
            Assert.AreEqual(wavelengths[4], spectralPlotParameters.Wavelengths[4]);
            Assert.AreEqual(TissueType.Skin, spectralPlotParameters.Tissue.TissueType);
        }

        [Test]
        public void Test_get_plot_data_mie()
        {
            var postData = "{\"spectralPlotType\":\"musp\",\"plotName\":\"μs'\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638},\"scatteringType\":\"Mie\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42},\"intralipidScatterer\":{\"volumeFraction\":0.01},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01},\"xAxis\":{\"axis\":\"wavelength\",\"axisRange\":{\"start\":650,\"stop\":1000,\"count\":36}}}";
            var spectralPlotParameters = JsonConvert.DeserializeObject<SpectralPlotParameters>(postData);
            _plotFactoryMock.Setup(x => x.GetPlot(PlotType.Spectral, spectralPlotParameters)).Returns("");
            var results = _spectralService.GetPlotData(spectralPlotParameters);
            Assert.AreEqual("", results);
            _plotFactoryMock.Verify(mock => mock.GetPlot(PlotType.Spectral, spectralPlotParameters), Times.Once);
        }

        [Test]
        public void Test_get_plot_data_intralipid()
        {
            var postData = "{\"spectralPlotType\":\"musp\",\"plotName\":\"μs'\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638},\"scatteringType\":\"Intralipid\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42},\"intralipidScatterer\":{\"volumeFraction\":0.01},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01},\"xAxis\":{\"axis\":\"wavelength\",\"axisRange\":{\"start\":650,\"stop\":1000,\"count\":36}}}";
            var spectralPlotParameters = JsonConvert.DeserializeObject<SpectralPlotParameters>(postData);
            _plotFactoryMock.Setup(x => x.GetPlot(PlotType.Spectral, spectralPlotParameters)).Returns("");
            var results = _spectralService.GetPlotData(spectralPlotParameters);
            Assert.AreEqual("", results);
            _plotFactoryMock.Verify(mock => mock.GetPlot(PlotType.Spectral, spectralPlotParameters), Times.Once);
        }
    }
}
