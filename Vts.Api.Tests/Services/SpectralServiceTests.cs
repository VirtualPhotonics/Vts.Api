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
    class SpectralServiceTests
    {
        private SpectralService _spectralService;
        private ILoggerFactory _factory;
        private ILogger<SpectralService> _logger;
        private Mock<IPlotFactory> _plotFactoryMock;

        [Test]
        public void Test_get_plot_data()
        {
            _plotFactoryMock = new Mock<IPlotFactory>();
            _plotFactoryMock.Setup(x => x.GetPlot(PlotType.Spectral, new SpectralPlotParameters())).Returns("");
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            _factory = serviceProvider.GetService<ILoggerFactory>()
                .AddConsole();
            _logger = _factory.CreateLogger<SpectralService>();
            _spectralService = new SpectralService(_logger, _plotFactoryMock.Object);
            var postData = "{\"plotType\":\"mua\",\"plotName\":\"μa\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638,\"visible\":true},\"scattererType\":\"PowerLaw\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42,\"show\":true},\"intralipidScatterer\":{\"volumeFraction\":0.01,\"show\":false},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01,\"show\":false},\"xAxis\":{\"title\":\"Wavelength Range\",\"startLabel\":\"Begin\",\"startLabelUnits\":\"nm\",\"start\":650,\"endLabel\":\"End\",\"endLabelUnits\":\"nm\",\"stop\":1000,\"numberLabel\":\"Number\",\"count\":36}}";
            var results = _spectralService.GetPlotData(JsonConvert.DeserializeObject<SpectralPlotParameters>(postData));
            Assert.IsNull(results);
        }
    }
}
