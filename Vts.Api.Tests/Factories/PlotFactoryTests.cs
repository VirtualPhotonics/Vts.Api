using System;
using System.Collections.Generic;
using System.Linq;
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
    class PlotFactoryTests
    {
        private ILoggerFactory _factory;
        private ILogger _logger;
        private IPlotFactory _plotFactory;
        private IServiceProvider _serviceProvider;

        [OneTimeSetUp]
        public void One_time_setup()
        {
            _serviceProvider = new ServiceCollection()
                .AddTransient<IParameterTools, ParameterTools>()
                .AddTransient<PlotSpectralResultsService>()
                .AddTransient<PlotSolutionDomainResultsService>()
                .AddLogging()
                .BuildServiceProvider();
            _factory = _serviceProvider.GetService<ILoggerFactory>()
                .AddConsole();
            _logger = _factory.CreateLogger<PlotFactory>();
        }

        [Test]
        public void Get_spectral_service_from_factory()
        {
            var postData = "{\"spectralPlotType\":\"mua\",\"plotName\":\"μa\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638,\"visible\":true},\"scatteringType\":\"PowerLaw\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42,\"show\":true},\"intralipidScatterer\":{\"volumeFraction\":0.01,\"show\":false},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01,\"show\":false},\"xAxis\":{\"title\":\"Wavelength Range\",\"startLabel\":\"Begin\",\"startLabelUnits\":\"nm\",\"start\":650,\"endLabel\":\"End\",\"endLabelUnits\":\"nm\",\"stop\":1000,\"numberLabel\":\"Number\",\"count\":36}}";
            var spectralPlotParameters = JsonConvert.DeserializeObject<SpectralPlotParameters>(postData);
            var xAxis = new DoubleRange(650, 1000, 36);
            spectralPlotParameters.YAxis = spectralPlotParameters.PlotName;
            spectralPlotParameters.Wavelengths = xAxis.AsEnumerable().ToArray();
            var chromophoreAbsorbers = new List<IChromophoreAbsorber>();
            foreach (var absorber in spectralPlotParameters.AbsorberConcentration)
            {
                chromophoreAbsorbers.Add(new ChromophoreAbsorber(Enum.Parse<ChromophoreType>(absorber.Label, true), absorber.Value));
            }
            var scatterer = new PowerLawScatterer(1.2, 1.42);
            spectralPlotParameters.Tissue = new Tissue(chromophoreAbsorbers, scatterer, spectralPlotParameters.TissueType);
            _plotFactory = new PlotFactory(_serviceProvider);
            var data = _plotFactory.GetPlot(PlotType.Spectral, spectralPlotParameters);
            var muaResults = "{\"Id\":\"SpectralMua\",\"PlotList\":[{\"Label\":\"Skin μa\",\"Data\":[[650.0,0.16765660516524314],[660.0,0.15671650246614327],[670.0,0.14697135542801],[680.0,0.13799908824769305],[690.0,0.12973522277796562],[700.0,0.12260932419347584],[710.0,0.11583351729772583],[720.0,0.11010170400729173],[730.0,0.10472095509441402],[740.0,0.10129111493244414],[750.0,0.099521779546167088],[760.0,0.096959293661164639],[770.0,0.092068432646702891],[780.0,0.087311046148140767],[790.0,0.082850035254262877],[800.0,0.079106164468758858],[810.0,0.076218507603072],[820.0,0.07371667586209707],[830.0,0.0719944590933807],[840.0,0.070298642060247829],[850.0,0.0683344235938162],[860.0,0.066498798013490928],[870.0,0.06488501473536043],[880.0,0.063512933452956943],[890.0,0.062254209691212314],[900.0,0.061061385509031681],[910.0,0.060018918288031978],[920.0,0.059569360145074537],[930.0,0.060352136653759242],[940.0,0.062968288552839938],[950.0,0.06862831638709066],[960.0,0.07645596009583773],[970.0,0.077403147687485074],[980.0,0.075685057055814575],[990.0,0.071645012147578471],[1000.0,0.066587976157675921]]}]}";
            Assert.AreEqual(muaResults, data);
        }

        [Test]
        public void Get_solution_domain_service_from_factory()
        {
            var postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRho\",\"xAxis\":{\"axis\":\"rho\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":19}},\"opticalProperties\":{\"title\":\"Optical Properties\",\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            _plotFactory = new PlotFactory(_serviceProvider);
            var data = _plotFactory.GetPlot(PlotType.SolutionDomain, solutionDomainPlotParameters);
            var results = "{\"Id\":\"ROfRho\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1\",\"Data\":[[0.5,0.046625597794768131],[1.0,0.020915537635778064],[1.5,0.012316866807976681],[2.0,0.0080842309517170675],[2.5,0.005623379122941038],[3.0,0.0040562789854642293],[3.5,0.0030006454971476613],[4.0,0.0022621931768063925],[4.5,0.0017313878247261138],[5.0,0.001341834630580925],[5.5,0.00105114060787151],[6.0,0.00083118079655800016],[6.5,0.00066274418610378413],[7.0,0.00053240356107749358],[7.5,0.00043059663530889934],[8.0,0.00035040528102366],[8.5,0.00028675569244185416],[9.0,0.000235881710029713],[9.5,0.00019495753939373391]]}]}";
            Assert.AreEqual(results, data);
        }
    }
}
