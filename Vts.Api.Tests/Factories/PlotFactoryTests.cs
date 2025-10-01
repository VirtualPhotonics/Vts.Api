using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using Vts.Api.Enums;
using Vts.Api.Factories;
using Vts.Api.Models;
using Vts.Api.Services;
using Vts.Api.Tools;
using Vts.Common;
using Vts.SpectralMapping;

namespace Vts.Api.Tests.Factories
{
    internal class PlotFactoryTests
    {
        private IPlotFactory? _plotFactory;
        private IServiceProvider? _serviceProvider;

        [OneTimeSetUp]
        public void One_time_setup()
        {
            _serviceProvider = new ServiceCollection()
                .AddTransient<IParameterTools, ParameterTools>()
                .AddTransient<PlotSpectralResultsService>()
                .AddTransient<PlotSolutionDomainResultsService>()
                .AddLogging()
                .BuildServiceProvider();
            _serviceProvider.GetService<ILoggerFactory>();
        }

        [Test]
        public void Get_spectral_service_from_factory()
        {
            const string postData = "{\"spectralPlotType\":\"mua\",\"plotName\":\"μa\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638,\"visible\":true},\"scatteringType\":\"PowerLaw\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42,\"show\":true},\"intralipidScatterer\":{\"volumeFraction\":0.01,\"show\":false},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01,\"show\":false},\"xAxis\":{\"title\":\"Wavelength Range\",\"startLabel\":\"Begin\",\"startLabelUnits\":\"nm\",\"start\":650,\"endLabel\":\"End\",\"endLabelUnits\":\"nm\",\"stop\":1000,\"numberLabel\":\"Number\",\"count\":36}}";
            var spectralPlotParameters = JsonConvert.DeserializeObject<SpectralPlotParameters>(postData);
            var xAxis = new DoubleRange(650, 1000, 36);
            Assert.That(spectralPlotParameters, Is.Not.Null);
            if (spectralPlotParameters == null) return;
            spectralPlotParameters.YAxis = spectralPlotParameters.PlotName;
            spectralPlotParameters.Wavelengths = xAxis.AsEnumerable().ToArray();
            var chromophoreAbsorbers = spectralPlotParameters.AbsorberConcentration
                .Select(absorber =>
                    new ChromophoreAbsorber(Enum.Parse<ChromophoreType>(absorber.Label, true), absorber.Value))
                .Cast<IChromophoreAbsorber>().ToList();
            var scatterer = new PowerLawScatterer(1.2, 1.42);
            spectralPlotParameters.Tissue =
                new Tissue(chromophoreAbsorbers, scatterer, spectralPlotParameters.TissueType);
            _plotFactory = new PlotFactory(_serviceProvider);
            var data = _plotFactory.GetPlot(PlotType.Spectral, spectralPlotParameters);
            const string muaResults =
                "{\"Id\":\"SpectralMua\",\"PlotList\":[{\"Label\":\"Skin μa\",\"Data\":[[650.0,0.16761442730524312],[660.0,0.15669646601814327],[670.0,0.14694852311101],[680.0,0.13797605595269305],[690.0,0.12970816782896563],[700.0,0.12257851943747583],[710.0,0.11593905529772583],[720.0,0.11006901145729174],[730.0,0.10470716381941403],[740.0,0.10129713518744414],[750.0,0.09952045051516709],[760.0,0.09695761565016461],[770.0,0.0920762047377029],[780.0,0.08731405783114075],[790.0,0.08294564675426289],[800.0,0.07919530416875885],[810.0,0.076214515206072],[820.0,0.07369000627009706],[830.0,0.07200142439938069],[840.0,0.07031012389624783],[850.0,0.06833351354281621],[860.0,0.06649372224849093],[870.0,0.06486856682636044],[880.0,0.06352665562045694],[890.0,0.062242237998012304],[900.0,0.06106796490213168],[910.0,0.060012233277831985],[920.0,0.05949638364057454],[930.0,0.060484228760059246],[940.0,0.06256515698203993],[950.0,0.06861256761619067],[960.0,0.07653394216643772],[970.0,0.07739838935008508],[980.0,0.07569121557171457],[990.0,0.07174238370307848],[1000.0,0.0666131765095759]]}]}";
            Assert.That(JsonConvert.SerializeObject(data), Is.EqualTo(muaResults));
        }

        [Test]
        public void Get_solution_domain_service_from_factory()
        {
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRho\",\"xAxis\":{\"axis\":\"rho\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":19}},\"opticalProperties\":{\"title\":\"Optical Properties\",\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            _plotFactory = new PlotFactory(_serviceProvider);
            var data = _plotFactory.GetPlot(PlotType.SolutionDomain, solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfRho\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1\",\"Data\":[[0.5,0.04662559779476813],[1.0,0.020915537635778064],[1.5,0.012316866807976681],[2.0,0.008084230951717068],[2.5,0.005623379122941038],[3.0,0.004056278985464229],[3.5,0.0030006454971476613],[4.0,0.0022621931768063925],[4.5,0.0017313878247261138],[5.0,0.001341834630580925],[5.5,0.00105114060787151],[6.0,0.0008311807965580002],[6.5,0.0006627441861037841],[7.0,0.0005324035610774936],[7.5,0.00043059663530889934],[8.0,0.00035040528102366],[8.5,0.00028675569244185416],[9.0,0.000235881710029713],[9.5,0.0001949575393937339]]}]}";
            Assert.That(JsonConvert.SerializeObject(data), Is.EqualTo(results));
        }

        [Test]
        public void GetPlot_returns_null()
        {
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRho\",\"xAxis\":{\"axis\":\"rho\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":19}},\"opticalProperties\":{\"title\":\"Optical Properties\",\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            _plotFactory = new PlotFactory(_serviceProvider);
            var data = _plotFactory.GetPlot((PlotType)99, solutionDomainPlotParameters);
            Assert.That(data, Is.Null);
        }
    }
}
