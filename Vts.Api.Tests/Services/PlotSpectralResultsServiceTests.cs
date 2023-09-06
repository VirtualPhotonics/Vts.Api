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
            const string muaResults = "{\"Id\":\"SpectralMua\",\"PlotList\":[{\"Label\":\"Skin μa\",\"Data\":[[650.0,0.16765660516524314],[660.0,0.15671650246614327],[670.0,0.14697135542801],[680.0,0.13799908824769305],[690.0,0.12973522277796562],[700.0,0.12260932419347584],[710.0,0.11583351729772583],[720.0,0.11010170400729173],[730.0,0.10472095509441402],[740.0,0.10129111493244414],[750.0,0.099521779546167088],[760.0,0.096959293661164639],[770.0,0.092068432646702891],[780.0,0.087311046148140767],[790.0,0.082850035254262877],[800.0,0.079106164468758858],[810.0,0.076218507603072],[820.0,0.07371667586209707],[830.0,0.0719944590933807],[840.0,0.070298642060247829],[850.0,0.0683344235938162],[860.0,0.066498798013490928],[870.0,0.06488501473536043],[880.0,0.063512933452956943],[890.0,0.062254209691212314],[900.0,0.061061385509031681],[910.0,0.060018918288031978],[920.0,0.059569360145074537],[930.0,0.060352136653759242],[940.0,0.062968288552839938],[950.0,0.06862831638709066],[960.0,0.07645596009583773],[970.0,0.077403147687485074],[980.0,0.075685057055814575],[990.0,0.071645012147578471],[1000.0,0.066587976157675921]]}]}";
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
            Assert.AreEqual(expected?.Id, data.Id);
            Assert.AreEqual(expected?.PlotList.Count, data.PlotList.Count);
            for (var i = 0; i < expected?.PlotList.Count; i++)
            {
                Assert.AreEqual(expected.PlotList[i].Label, data.PlotList[i].Label);
                Assert.AreEqual(expected.PlotList[i].Data.Count, data.PlotList[i].Data.Count);
                for (var j = 0; j < expected.PlotList[i].Data.Count; j++)
                {
                    Assert.That(Math.Round(expected.PlotList[i].Data[j][0], 6), Is.EqualTo(Math.Round(data.PlotList[i].Data[j][0], 6)));
                    Assert.That(Math.Round(expected.PlotList[i].Data[j][1], 6), Is.EqualTo(Math.Round(data.PlotList[i].Data[j][1], 6)));
                }
            }
        }
    }
}
