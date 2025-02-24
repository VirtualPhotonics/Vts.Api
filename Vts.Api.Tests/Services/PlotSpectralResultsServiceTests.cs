using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using Vts.Api.Data;
using Vts.Api.Models;
using Vts.Api.Services;
using Vts.Common;
using Vts.SpectralMapping;

namespace Vts.Api.Tests.Services
{
    internal class PlotSpectralResultsServiceTests
    {
        private PlotSpectralResultsService? _plotSpectralResultsService;
        private ILoggerFactory? _factory;
        private ILogger<PlotSpectralResultsService>? _logger;

        [OneTimeSetUp]
        public void One_time_setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            _factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = _factory.CreateLogger<PlotSpectralResultsService>();
        }

        [Test]
        public void Test_plot_mua()
        {
            _plotSpectralResultsService = new PlotSpectralResultsService(_logger);
            const string postData = "{\"spectralPlotType\":\"mua\",\"plotName\":\"μa\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638},\"scatteringType\":\"PowerLaw\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42},\"intralipidScatterer\":{\"volumeFraction\":0.01},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01},\"xAxis\":{\"start\":650,\"stop\":1000,\"count\":36}}";
            var spectralPlotParameters = JsonConvert.DeserializeObject<SpectralPlotParameters>(postData);
            var xAxis = new DoubleRange(650, 1000, 36);
            spectralPlotParameters.YAxis = spectralPlotParameters.PlotName;
            spectralPlotParameters.Wavelengths = xAxis.AsEnumerable().ToArray();
            var chromophoreAbsorbers = spectralPlotParameters.AbsorberConcentration.Select(absorber => new ChromophoreAbsorber(Enum.Parse<ChromophoreType>(absorber.Label, true), absorber.Value)).Cast<IChromophoreAbsorber>().ToList();
            var scatterer = new PowerLawScatterer(1.2, 1.42);
            spectralPlotParameters.Tissue = new Tissue(chromophoreAbsorbers, scatterer, spectralPlotParameters.TissueType);
            var data = _plotSpectralResultsService.Plot(spectralPlotParameters);
            const string muaResults = "{\"id\":\"SpectralMua\",\"plotList\":[{\"label\":\"Skin μa\",\"data\":[[650.0,0.16761442730524312],[660.0,0.15669646601814327],[670.0,0.14694852311101],[680.0,0.13797605595269305],[690.0,0.12970816782896563],[700.0,0.12257851943747583],[710.0,0.11593905529772583],[720.0,0.11006901145729174],[730.0,0.10470716381941403],[740.0,0.10129713518744414],[750.0,0.09952045051516709],[760.0,0.09695761565016461],[770.0,0.0920762047377029],[780.0,0.08731405783114075],[790.0,0.08294564675426289],[800.0,0.07919530416875885],[810.0,0.076214515206072],[820.0,0.07369000627009706],[830.0,0.07200142439938069],[840.0,0.07031012389624783],[850.0,0.06833351354281621],[860.0,0.06649372224849093],[870.0,0.06486856682636044],[880.0,0.06352665562045694],[890.0,0.062242237998012304],[900.0,0.06106796490213168],[910.0,0.060012233277831985],[920.0,0.05949638364057454],[930.0,0.060484228760059246],[940.0,0.06256515698203993],[950.0,0.06861256761619067],[960.0,0.07653394216643772],[970.0,0.07739838935008508],[980.0,0.07569121557171457],[990.0,0.07174238370307848],[1000.0,0.0666131765095759]]}]}";
            Verify_plot_data(data, muaResults);
        }


        [Test]
        public void Test_plot_musp()
        {
            _plotSpectralResultsService = new PlotSpectralResultsService(_logger);
            const string postData = "{\"spectralPlotType\":\"musp\",\"plotName\":\"μs'\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638},\"scatteringType\":\"PowerLaw\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42},\"intralipidScatterer\":{\"volumeFraction\":0.01},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01},\"xAxis\":{\"start\":650,\"stop\":1000,\"count\":36}}";
            var spectralPlotParameters = JsonConvert.DeserializeObject<SpectralPlotParameters>(postData);
            var xAxis = new DoubleRange(650, 1000, 36);
            spectralPlotParameters.YAxis = spectralPlotParameters.PlotName;
            spectralPlotParameters.Wavelengths = xAxis.AsEnumerable().ToArray();
            var chromophoreAbsorbers = spectralPlotParameters.AbsorberConcentration.Select(absorber => new ChromophoreAbsorber(Enum.Parse<ChromophoreType>(absorber.Label, true), absorber.Value)).Cast<IChromophoreAbsorber>().ToList();
            var scatterer = new PowerLawScatterer(1.2, 1.42);
            spectralPlotParameters.Tissue = new Tissue(chromophoreAbsorbers, scatterer, spectralPlotParameters.TissueType);
            var data = _plotSpectralResultsService.Plot(spectralPlotParameters);
            const string muspResults = "{\"Id\":\"SpectralMusp\",\"PlotList\":[{\"Label\":\"Skin μs'\",\"Data\":[[650.0,2.2123013258570863],[660.0,2.1648552221223785],[670.0,2.1191174426328576],[680.0,2.0750023361697605],[690.0,2.0324297212859692],[700.0,1.9913244640761809],[710.0,1.9516160940115432],[720.0,1.9132384539276279],[730.0,1.8761293807044339],[740.0,1.8402304135697998],[750.0,1.8054865273011322],[760.0,1.771845887901413],[770.0,1.7392596285897868],[780.0,1.7076816441795259],[790.0,1.6770684021210169],[800.0,1.6473787686682118],[810.0,1.618573848786806],[820.0,1.5906168385639152],[830.0,1.5634728890045213],[840.0,1.5371089802114051],[850.0,1.5114938050444233],[860.0,1.486597661443283],[870.0,1.4623923526767395],[880.0,1.438851094851493],[890.0,1.415948431076983],[900.0,1.3936601517386393],[910.0,1.3719632203826795],[920.0,1.3508357047609152],[930.0,1.330256712624823],[940.0,1.3102063318948318],[950.0,1.290665574863872],[960.0,1.2716163261240603],[970.0,1.2530412939323394],[980.0,1.2349239647552681],[990.0,1.2172485607551906],[1000.0,1.2]]}]}";
            Verify_plot_data(data, muspResults);
        }

        internal static void Verify_plot_data(Plots data, string results)
        {
            var expected = JsonConvert.DeserializeObject<Plots>(results);
            Assert.That(data.Id, Is.EqualTo(expected?.Id));
            Assert.That(data.PlotList.Count, Is.EqualTo(expected?.PlotList.Count));
            for (var i = 0; i < expected?.PlotList.Count; i++)
            {
                Assert.That(data.PlotList[i].Label, Is.EqualTo(expected.PlotList[i].Label));
                Assert.That(data.PlotList[i].Data.Count, Is.EqualTo(expected.PlotList[i].Data.Count));
                for (var j = 0; j < expected.PlotList[i].Data.Count; j++)
                {
                    Assert.That(Math.Round(expected.PlotList[i].Data[j][0], 6), Is.EqualTo(Math.Round(data.PlotList[i].Data[j][0], 6)));
                    Assert.That(Math.Round(expected.PlotList[i].Data[j][1], 6), Is.EqualTo(Math.Round(data.PlotList[i].Data[j][1], 6)));
                }
            }
        }
    }
}
