﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using Vts.Api.Data;
using Vts.Api.Models;
using Vts.Api.Services;
using Vts.Api.Tools;

namespace Vts.Api.Tests.Services
{
    internal class PlotSolutionDomainResultsServiceTests
    {
        private ILoggerFactory? _factory;
        private ILogger<PlotSolutionDomainResultsService>? _logger;
        private IServiceProvider? _serviceProvider;
        private PlotSolutionDomainResultsService? _plotSolutionDomainResultsService;

        [OneTimeSetUp]
        public void One_time_setup()
        {
            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            _factory = _serviceProvider.GetService<ILoggerFactory>();
            if (_factory != null) _logger = _factory.CreateLogger<PlotSolutionDomainResultsService>();
        }

        [Test]
        public void Plot_solution_domain_rofrho()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRho\",\"xAxis\":{\"axis\":\"rho\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":19}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfRho\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1\",\"Data\":[[0.5,0.046625597794768131],[1.0,0.020915537635778064],[1.5,0.012316866807976681],[2.0,0.0080842309517170675],[2.5,0.005623379122941038],[3.0,0.0040562789854642293],[3.5,0.0030006454971476613],[4.0,0.0022621931768063925],[4.5,0.0017313878247261138],[5.0,0.001341834630580925],[5.5,0.00105114060787151],[6.0,0.00083118079655800016],[6.5,0.00066274418610378413],[7.0,0.00053240356107749358],[7.5,0.00043059663530889934],[8.0,0.00035040528102366],[8.5,0.00028675569244185416],[9.0,0.000235881710029713],[9.5,0.00019495753939373391]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_rofrho_vs_wavelength()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"PointSourceSDA\",\"solutionDomain\":\"ROfRho\",\"xAxis\":{\"axis\":\"wavelength\",\"axisRange\":{\"start\":650,\"stop\":1000,\"count\":8}},\"independentAxis\":{\"axis\":\"rho\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":9}},\"wavelengthOpticalPropertyList\":[{\"mua\":0.0678524459053467,\"musp\":1.06457665884717,\"g\":0.8,\"n\":1.4},{\"mua\":0.0361315970667451,\"musp\":1.02205758134735,\"g\":0.8,\"n\":1.4},{\"mua\":0.0386397078314404,\"musp\":0.984001116480081,\"g\":0.8,\"n\":1.4},{\"mua\":0.0368351407773848,\"musp\":0.949685469521921,\"g\":0.8,\"n\":1.4},{\"mua\":0.0443735471868325,\"musp\":0.918541699382405,\"g\":0.8,\"n\":1.4},{\"mua\":0.0514456486132726,\"musp\":0.890114561696066,\"g\":0.8,\"n\":1.4},{\"mua\":0.0694976128306352,\"musp\":0.864034936500061,\"g\":0.8,\"n\":1.4},{\"mua\":0.0678098107455411,\"musp\":0.84,\"g\":0.8,\"n\":1.4}],\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfRho\",\"PlotList\":[{\"Label\":\"PointSourceSDA Rho=0.5\",\"Data\":[[650.0,0.037575594366308029],[700.0,0.036425341127494307],[750.0,0.034225954428999693],[800.0,0.0324167695064711],[850.0,0.030497157708222037],[900.0,0.0288226994243444],[950.0,0.027185646168625149],[1000.0,0.025998211719311357]]},{\"Label\":\"PointSourceSDA Rho=1.625\",\"Data\":[[650.0,0.00930611971108063],[700.0,0.010657585196511617],[750.0,0.010286186968902721],[800.0,0.010164293733480037],[850.0,0.0095858760090751765],[900.0,0.0090951247916137862],[950.0,0.00827559449445059],[1000.0,0.0081901878522137352]]},{\"Label\":\"PointSourceSDA Rho=2.75\",\"Data\":[[650.0,0.0029739205260132136],[700.0,0.0039355338028903173],[750.0,0.003814873671752708],[800.0,0.0038531053567852835],[850.0,0.0035692504470349116],[900.0,0.003336371090660979],[950.0,0.0028778505896115969],[1000.0,0.0028999195234677339]]},{\"Label\":\"PointSourceSDA Rho=3.875\",\"Data\":[[650.0,0.0011202344923379804],[700.0,0.0017114711547250518],[750.0,0.0016625284890804022],[800.0,0.0017127354060420855],[850.0,0.0015545742016347228],[900.0,0.001428309867090108],[950.0,0.0011656365810843558],[1000.0,0.0011930188488355085]]},{\"Label\":\"PointSourceSDA Rho=5\",\"Data\":[[650.0,0.00045827142014725564],[700.0,0.000809630138954956],[750.0,0.00078755286344886878],[800.0,0.00082730595991490849],[850.0,0.00073517708172734033],[900.0,0.00066356712975991917],[950.0,0.00051207793868213863],[1000.0,0.00053227539361280459]]},{\"Label\":\"PointSourceSDA Rho=6.125\",\"Data\":[[650.0,0.00019728294338474826],[700.0,0.00040373776522230852],[750.0,0.00039285283597287584],[800.0,0.00042060941292439553],[850.0,0.00036553410448836754],[900.0,0.00032386465894379134],[950.0,0.000236113847963527],[1000.0,0.0002492258245077268]]},{\"Label\":\"PointSourceSDA Rho=7.25\",\"Data\":[[650.0,8.80951583462366E-05],[700.0,0.00020915514848908389],[750.0,0.00020335456137745246],[800.0,0.00022177830753647253],[850.0,0.0001882572984469717],[900.0,0.00016357542248897285],[950.0,0.00011253082080027964],[1000.0,0.00012058878432062612]]},{\"Label\":\"PointSourceSDA Rho=8.375\",\"Data\":[[650.0,4.047385139570443E-05],[700.0,0.00011162789103384096],[750.0,0.00010833856351438841],[800.0,0.000120291245952519],[850.0,9.9619111007369024E-05],[900.0,8.4807365843274108E-05],[950.0,5.498748073778873E-05],[1000.0,5.9804005013378227E-05]]},{\"Label\":\"PointSourceSDA Rho=9.5\",\"Data\":[[650.0,1.9029153552775764E-05],[700.0,6.1037316122173826E-05],[750.0,5.9084853608892262E-05],[800.0,6.6760821528115492E-05],[850.0,5.3884734932340023E-05],[900.0,4.4907616457900632E-05],[950.0,2.7412746447384668E-05],[1000.0,3.0249618426184355E-05]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_rofrho_with_noise()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"PointSourceSDA\",\"solutionDomain\":\"ROfRho\",\"xAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":19}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"5\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfRho\",\"PlotList\":[{\"Label\":\"PointSourceSDA μa=0.01 μs'=1\",\"Data\":[[0.5,0.036717565366669314],[1.0,0.022495920023422971],[1.5,0.013879262176677631],[2.0,0.00909004259895382],[2.5,0.0062645286764352335],[3.0,0.0044801714205314185],[3.5,0.0032915341287590738],[4.0,0.0024679357796148146],[4.5,0.001880333047738071],[5.0,0.0014516116956410983],[5.5,0.0011332021316975245],[6.0,0.00089323725881868422],[6.5,0.00071013402604237666],[7.0,0.00056890403099352531],[7.5,0.00045892623723661589],[8.0,0.00037254747448076136],[8.5,0.00030417414834209468],[9.0,0.00024966707466955989],[9.5,0.00020592949690778192]]}]}";
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
                    Assert.AreNotEqual(expected.PlotList[i].Data[j][1], data.PlotList[i].Data[j][1]);
                }
            }
        }

        [Test]
        public void Plot_solution_domain_roffx()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfFx\",\"xAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0,\"stop\":0.5,\"count\":51}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfFx\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1\",\"Data\":[[0.0,0.63074956709022811],[0.01,0.61511390155697521],[0.02,0.57581355802666567],[0.03,0.52648940417289258],[0.04,0.47641975678131426],[0.05,0.42979057902116991],[0.060000000000000005,0.38795142872642363],[0.07,0.35100142943758456],[0.08,0.31856082282187059],[0.09,0.29010683461830877],[0.099999999999999992,0.26511137561912507],[0.10999999999999999,0.24309256134392782],[0.11999999999999998,0.22362953218809636],[0.12999999999999998,0.20636219945706194],[0.13999999999999999,0.19098535848765033],[0.15,0.17724124663989432],[0.16,0.16491224008023292],[0.17,0.15381432121162161],[0.18000000000000002,0.1437914820904212],[0.19000000000000003,0.13471103357958683],[0.20000000000000004,0.12645971747810142],[0.21000000000000005,0.1189405011724739],[0.22000000000000006,0.11206993977179741],[0.23000000000000007,0.10577600457791871],[0.24000000000000007,0.0999962925214448],[0.25000000000000006,0.094676546080505616],[0.26000000000000006,0.089769426187900223],[0.27000000000000007,0.085233491524545235],[0.28000000000000008,0.081032346534100033],[0.29000000000000009,0.077133927736687549],[0.3000000000000001,0.07350990375050763],[0.31000000000000011,0.070135169108853729],[0.32000000000000012,0.066987415710327611],[0.33000000000000013,0.064046768747402769],[0.34000000000000014,0.061295476373501349],[0.35000000000000014,0.058717644312130252],[0.36000000000000015,0.0562990081794465],[0.37000000000000016,0.05402673756006756],[0.38000000000000017,0.051889266905287647],[0.39000000000000018,0.049876149160825334],[0.40000000000000019,0.0479779287155903],[0.4100000000000002,0.046186030823694275],[0.42000000000000021,0.044492665112868431],[0.43000000000000022,0.042890741172571606],[0.44000000000000022,0.041373794529549104],[0.45000000000000023,0.03993592157958277],[0.46000000000000024,0.03857172226142766],[0.47000000000000025,0.037276249440327675],[0.48000000000000026,0.036044964120409452],[0.49000000000000027,0.034873695732827989],[0.50000000000000022,0.033758606853979467]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_rofrhoandtime_independent_time()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRhoAndTime\",\"independentAxis\":{\"axis\":\"time\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"rho\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":19}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfRhoAndTimeIndependentTime\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Time=0.05\",\"Data\":[[0.5,0.06585901221655005],[1.0,0.062455516676356737],[1.5,0.057169377087447348],[2.0,0.050511838592775582],[2.5,0.043078438756018522],[3.0,0.03546204655435764],[3.5,0.028177645294074079],[4.0,0.021611388071518672],[4.5,0.015999178653643323],[5.0,0.011432725490165871],[5.5,0.0078856759346756858],[6.0,0.0052500705229610849],[6.5,0.0033738705138895131],[7.0,0.0020928049128279666],[7.5,0.0012530434868286927],[8.0,0.0007241701219682287],[8.5,0.00040397279920643351],[9.0,0.00021752075729664121],[9.5,0.00011305410894769459]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_rofrhoandtime_independent_rho()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRhoAndTime\",\"independentAxis\":{\"axis\":\"rho\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"time\",\"axisRange\":{\"start\":0,\"stop\":0.05,\"count\":51}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfRhoAndTimeIndependentRho\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Rho=0.05\",\"Data\":[[0.0,0.0],[0.001,26.955024580168278],[0.002,9.6756331324655171],[0.003,5.3058751707911194],[0.004,3.4615354025712919],[0.005,2.4838749260174731],[0.006,1.8930661597509555],[0.007,1.5041824736281972],[0.008,1.232320671191083],[0.0090000000000000011,1.0335481714507895],[0.010000000000000002,0.88306001452358107],[0.011000000000000003,0.76589133477900018],[0.012000000000000004,0.67253368871036212],[0.013000000000000005,0.59668955294090287],[0.014000000000000005,0.53404378088669513],[0.015000000000000006,0.48155424633987243],[0.016000000000000007,0.43702358474129965],[0.017000000000000008,0.39883080889652978],[0.018000000000000009,0.36575771655226247],[0.01900000000000001,0.33687352768276324],[0.020000000000000011,0.31145639575317441],[0.021000000000000012,0.28893889247806936],[0.022000000000000013,0.26886944210495978],[0.023000000000000013,0.25088458514299738],[0.024000000000000014,0.23468872993380835],[0.025000000000000015,0.22003916702830331],[0.026000000000000016,0.206734837950831],[0.027000000000000017,0.19460781900993651],[0.028000000000000018,0.18351679335548815],[0.029000000000000019,0.17334199610023307],[0.03000000000000002,0.16398126272804622],[0.031000000000000021,0.15534691227599343],[0.032000000000000021,0.14736326818642576],[0.033000000000000022,0.13996467066741791],[0.034000000000000023,0.13309387113604398],[0.035000000000000024,0.12670072608053581],[0.036000000000000025,0.12074112735938694],[0.037000000000000026,0.11517612056166858],[0.038000000000000027,0.10997117398502895],[0.039000000000000028,0.10509556903668679],[0.040000000000000029,0.10052188913497521],[0.041000000000000029,0.096225588993417877],[0.04200000000000003,0.0921846298753212],[0.043000000000000031,0.088379169284806089],[0.044000000000000032,0.084791295809499026],[0.045000000000000033,0.081404801598928822],[0.046000000000000034,0.078204986361901943],[0.047000000000000035,0.075178487879279984],[0.048000000000000036,0.072313134918989191],[0.049000000000000037,0.069597819156060031],[0.050000000000000037,0.067022383279113432]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_rofrhoandtime_independent_rho_multi_axis()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRhoAndTime\",\"xAxis\":{\"axis\":\"time\",\"axisRange\":{\"start\":0,\"stop\":0.05,\"count\":11}},\"independentAxis\":{\"axis\":\"rho\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":3}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfRhoAndTimeIndependentRho\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Rho=0.5\",\"Data\":[[0.0,0.0],[0.005,2.08489016059789],[0.01,0.8090343257258511],[0.015,0.45425149878676496],[0.02,0.29811622803639659],[0.025,0.2124666075086156],[0.030000000000000002,0.15926480067287857],[0.035,0.12357064448003986],[0.04,0.098345574994098919],[0.045,0.079836300875322452],[0.049999999999999996,0.065859012216550078]]},{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Rho=5\",\"Data\":[[0.0,0.0],[0.005,5.1811815599860374E-08],[0.01,0.00012753804748896541],[0.015,0.001325597204067443],[0.02,0.0037430189467746195],[0.025,0.0064026507070737418],[0.030000000000000002,0.0086035444662990318],[0.035,0.010128289728665743],[0.04,0.011019782625630099],[0.045,0.011408769601298671],[0.049999999999999996,0.011432725490165871]]},{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Rho=9.5\",\"Data\":[[0.0,0.0],[0.005,4.6321097617379654E-28],[0.01,1.2059884009762725E-14],[0.015,2.7512718075746295E-10],[0.02,3.6396821969573962E-08],[0.025,6.2608451274312E-07],[0.030000000000000002,3.9195694761089127E-06],[0.035,1.3849864033879281E-05],[0.04,3.4363211666172372E-05],[0.045,6.7548044768626466E-05],[0.049999999999999996,0.00011305410894769455]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_ROfFxAndFt_independent_fx_wavelength()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfFxAndFt\",\"xAxis\":{\"axis\":\"wavelength\",\"axisRange\":{\"start\":650,\"stop\":1000,\"count\":8}},\"independentAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0,\"stop\":0.5,\"count\":9}},\"secondIndependentAxis\":{\"axis\":\"ft\",\"axisValue\":0},\"wavelengthOpticalPropertyList\":[{\"mua\":0.0678524459053467,\"musp\":1.06457665884717,\"g\":0.8,\"n\":1.4},{\"mua\":0.0361315970667451,\"musp\":1.02205758134735,\"g\":0.8,\"n\":1.4},{\"mua\":0.0386397078314404,\"musp\":0.984001116480081,\"g\":0.8,\"n\":1.4},{\"mua\":0.0368351407773848,\"musp\":0.949685469521921,\"g\":0.8,\"n\":1.4},{\"mua\":0.0443735471868325,\"musp\":0.918541699382405,\"g\":0.8,\"n\":1.4},{\"mua\":0.0514456486132726,\"musp\":0.890114561696066,\"g\":0.8,\"n\":1.4},{\"mua\":0.0694976128306352,\"musp\":0.864034936500061,\"g\":0.8,\"n\":1.4},{\"mua\":0.0678098107455411,\"musp\":0.84,\"g\":0.8,\"n\":1.4}],\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"id\":\"ROfFxAndFtIndependentFx\",\"plotList\":[{\"label\":\"DistributedPointSourceSDA Fx=0 Ft=0(real)\",\"data\":[[650,1.8900073999013092],[700,2.118427400018454],[750,2.080423557147811],[800,2.0849460649560974],[850,2.0019964591579362],[900,1.9304472495462708],[950,1.7899870578164037],[1000,1.788387144675943]]},{\"label\":\"DistributedPointSourceSDA Fx=0 Ft=0(imag)\",\"data\":[[650,0],[700,0],[750,0],[800,0],[850,0],[900,0],[950,0],[1000,0]]},{\"label\":\"DistributedPointSourceSDA Fx=0.0625 Ft=0(real)\",\"data\":[[650,1.715985042688596],[700,1.8425774896829576],[750,1.8065572271603911],[800,1.7944567483077716],[850,1.7358190924909298],[900,1.68302015748852],[950,1.5872051642676026],[1000,1.577062664757622]]},{\"label\":\"DistributedPointSourceSDA Fx=0.0625 Ft=0(imag)\",\"data\":[[650,0],[700,0],[750,0],[800,0],[850,0],[900,0],[950,0],[1000,0]]},{\"label\":\"DistributedPointSourceSDA Fx=0.125 Ft=0(real)\",\"data\":[[650,1.4248101833414268],[700,1.4682951074863289],[750,1.4341661411437399],[800,1.4118905651197908],[850,1.370260861142619],[900,1.332048297106874],[950,1.274161177768572],[1000,1.257478267369169]]},{\"label\":\"DistributedPointSourceSDA Fx=0.125 Ft=0(imag)\",\"data\":[[650,0],[700,0],[750,0],[800,0],[850,0],[900,0],[950,0],[1000,0]]},{\"label\":\"DistributedPointSourceSDA Fx=0.1875 Ft=0(real)\",\"data\":[[650,1.1785715438309399],[700,1.1870800282150926],[750,1.1553151206897565],[800,1.130709862845734],[850,1.097314937104921],[900,1.0665583177095588],[950,1.0264890364015375],[1000,1.0082957868168092]]},{\"label\":\"DistributedPointSourceSDA Fx=0.1875 Ft=0(imag)\",\"data\":[[650,0],[700,0],[750,0],[800,0],[850,0],[900,0],[950,0],[1000,0]]},{\"label\":\"DistributedPointSourceSDA Fx=0.25 Ft=0(real)\",\"data\":[[650,0.9888484970379409],[700,0.982005387052902],[750,0.9528282501803808],[800,0.9286067080092643],[850,0.9003518553663331],[900,0.8743395561102318],[950,0.8438764081243718],[1000,0.8261127512727281]]},{\"label\":\"DistributedPointSourceSDA Fx=0.25 Ft=0(imag)\",\"data\":[[650,0],[700,0],[750,0],[800,0],[850,0],[900,0],[950,0],[1000,0]]},{\"label\":\"DistributedPointSourceSDA Fx=0.3125 Ft=0(real)\",\"data\":[[650,0.84270800369951],[700,0.8287572188080379],[750,0.8020858659751241],[800,0.7791521092112554],[850,0.7546066620704881],[900,0.7320398506907679],[950,0.7075369217677994],[1000,0.6908401205270217]]},{\"label\":\"DistributedPointSourceSDA Fx=0.3125 Ft=0(imag)\",\"data\":[[650,0],[700,0],[750,0],[800,0],[850,0],[900,0],[950,0],[1000,0]]},{\"label\":\"DistributedPointSourceSDA Fx=0.375 Ft=0(real)\",\"data\":[[650,0.7282185111643771],[700,0.710978836333275],[750,0.6866060295926086],[800,0.6652146259007493],[850,0.6435473860283096],[900,0.6236577332612331],[950,0.6032303685524381],[1000,0.5877590269301294]]},{\"label\":\"DistributedPointSourceSDA Fx=0.375 Ft=0(imag)\",\"data\":[[650,0],[700,0],[750,0],[800,0],[850,0],[900,0],[950,0],[1000,0]]},{\"label\":\"DistributedPointSourceSDA Fx=0.4375 Ft=0(real)\",\"data\":[[650,0.63680978145372],[700,0.6181894787370624],[750,0.5958773884371141],[800,0.5760362068709812],[850,0.5566936740684499],[900,0.5389652333487972],[950,0.5215148098084617],[1000,0.507249772280999]]},{\"label\":\"DistributedPointSourceSDA Fx=0.4375 Ft=0(imag)\",\"data\":[[650,0],[700,0],[750,0],[800,0],[850,0],[900,0],[950,0],[1000,0]]},{\"label\":\"DistributedPointSourceSDA Fx=0.5 Ft=0(real)\",\"data\":[[650,0.5625378942654524],[700,0.543539869846358],[750,0.5230589127389039],[800,0.5046823237931978],[850,0.48726557424651235],[900,0.4713252485434585],[950,0.456152078522004],[1000,0.4430106211624478]]},{\"label\":\"DistributedPointSourceSDA Fx=0.5 Ft=0(imag)\",\"data\":[[650,0],[700,0],[750,0],[800,0],[850,0],[900,0],[950,0],[1000,0]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_roffxandtime_independent_time()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfFxAndTime\",\"independentAxis\":{\"axis\":\"time\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0,\"stop\":0.05,\"count\":51}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfFxAndTimeIndependentTime\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Time=0.05\",\"Data\":[[0.0,2.9766418851506584],[0.001,2.9762266693206],[0.002,2.9749813630254529],[0.003,2.9729070103342186],[0.004,2.9700053462526586],[0.005,2.9662787964017485],[0.006,2.9617304736385224],[0.007,2.956364173722748],[0.008,2.9501843700378614],[0.0090000000000000011,2.9431962073765581],[0.010000000000000002,2.9354054948028874],[0.011000000000000003,2.926818697604936],[0.012000000000000004,2.9174429283536685],[0.013000000000000005,2.9072859370850921],[0.014000000000000005,2.8963561006247942],[0.015000000000000006,2.8846624110757917],[0.016000000000000007,2.8722144634908093],[0.017000000000000008,2.859022442754092],[0.018000000000000009,2.8450971096968356],[0.01900000000000001,2.830449786472558],[0.020000000000000011,2.8150923412212068],[0.021000000000000012,2.7990371720506007],[0.022000000000000013,2.7822971903649152],[0.023000000000000013,2.7648858035720569],[0.024000000000000014,2.7468168972026952],[0.025000000000000015,2.728104816473945],[0.026000000000000016,2.7087643473316252],[0.027000000000000017,2.6888106970062911],[0.028000000000000018,2.668259474119075],[0.029000000000000019,2.6471266683737045],[0.03000000000000002,2.6254286298714051],[0.031000000000000021,2.6031820480859196],[0.032000000000000021,2.580403930536443],[0.033000000000000022,2.5571115811966276],[0.034000000000000023,2.533322578677947],[0.035000000000000024,2.5090547542257196],[0.036000000000000025,2.48432616956604],[0.037000000000000026,2.4591550946418379],[0.038000000000000027,2.4335599852762155],[0.039000000000000028,2.4075594608010409],[0.040000000000000029,2.3811722816884431],[0.041000000000000029,2.3544173272224453],[0.04200000000000003,2.327313573247455],[0.043000000000000031,2.2998800700297606],[0.044000000000000032,2.2721359202675804],[0.045000000000000033,2.2441002572845234],[0.046000000000000034,2.2157922234405802],[0.047000000000000035,2.1872309487939785],[0.048000000000000036,2.1584355300463005],[0.049000000000000037,2.1294250098023229],[0.050000000000000037,2.1002183561750152]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_roffxandtime_independent_fx()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfFxAndTime\",\"independentAxis\":{\"axis\":\"fx\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"time\",\"axisRange\":{\"start\":0,\"stop\":0.05,\"count\":51}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfFxAndTimeIndependentFx\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Fx=0.05\",\"Data\":[[0.0,0.0],[0.001,23.983420749348422],[0.002,17.022806900535706],[0.003,13.884514605320298],[0.004,11.984830148269269],[0.005,10.670421191252846],[0.006,9.6881668567248731],[0.007,8.9166648368843084],[0.008,8.289336529957211],[0.0090000000000000011,7.7659876748676462],[0.010000000000000002,7.320517226604319],[0.011000000000000003,6.9350121990738138],[0.012000000000000004,6.5966636203580249],[0.013000000000000005,6.2960326624389022],[0.014000000000000005,6.0260084702841814],[0.015000000000000006,5.7811439581757194],[0.016000000000000007,5.5572114261665986],[0.017000000000000008,5.35089365562801],[0.018000000000000009,5.1595628539532177],[0.01900000000000001,4.9811190005927113],[0.020000000000000011,4.8138697038987175],[0.021000000000000012,4.6564398047552524],[0.022000000000000013,4.5077027043148679],[0.023000000000000013,4.3667277857457227],[0.024000000000000014,4.2327398912179239],[0.025000000000000015,4.1050879080515568],[0.026000000000000016,3.9832202872826286],[0.027000000000000017,3.86666587014492],[0.028000000000000018,3.7550188003199114],[0.029000000000000019,3.647926596366609],[0.03000000000000002,3.5450806793397232],[0.031000000000000021,3.4462088159246203],[0.032000000000000021,3.3510690620897234],[0.033000000000000022,3.2594448867845873],[0.034000000000000023,3.1711412272282495],[0.035000000000000024,3.0859812824381914],[0.036000000000000025,3.0038038939869889],[0.037000000000000026,2.9244613956242445],[0.038000000000000027,2.8478178386731621],[0.039000000000000028,2.7737475197398469],[0.040000000000000029,2.702133752570508],[0.041000000000000029,2.6328678378521229],[0.04200000000000003,2.5658481941334843],[0.043000000000000031,2.5009796204249355],[0.044000000000000032,2.4381726668607007],[0.045000000000000033,2.3773430944193086],[0.046000000000000034,2.3184114083588305],[0.047000000000000035,2.2613024529391095],[0.048000000000000036,2.2059450573312418],[0.049000000000000037,2.1522717244790943],[0.050000000000000037,2.1002183561750152]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_rofrhoandft_independent_ft()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRhoAndFt\",\"independentAxis\":{\"axis\":\"ft\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"rho\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":19}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfRhoAndFtIndependentFt\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Ft=0.05(real)\",\"Data\":[[0.5,0.046622228868063054],[1.0,0.020912242729702133],[1.5,0.012313679569634058],[2.0,0.00808117473513468],[2.5,0.0056204690607412327],[3.0,0.0040535237911984634],[3.5,0.002998048978911024],[4.0,0.0022597554734927229],[4.5,0.0017291063932551244],[5.0,0.0013397050201096783],[5.5,0.0010491570581324097],[6.0,0.00082933669465667958],[6.5,0.000661032411464029],[7.0,0.00053081674312182561],[7.5,0.00042912734367297524],[8.0,0.0003490461639517493],[8.5,0.00028549957558121825],[9.0,0.00023472166509051949],[9.5,0.00019388692997455379]]},{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Ft=0.05(imag)\",\"Data\":[[0.5,-0.00019143644524373756],[1.0,-0.00016684135831725942],[1.5,-0.00014496228779428769],[2.0,-0.0001257303682247569],[2.5,-0.00010895671662774741],[3.0,-9.4406928993085452E-05],[3.5,-8.1833066085944809E-05],[4.0,-7.0991846137201712E-05],[4.5,-6.1655452734933817E-05],[5.0,-5.3617279907957434E-05],[5.5,-4.6694135946912484E-05],[6.0,-4.0726139916080786E-05],[6.5,-3.5575269001414369E-05],[7.0,-3.1123235478952438E-05],[7.5,-2.7269133011238287E-05],[8.0,-2.3927112878402039E-05],[8.5,-2.1024228791233023E-05],[9.0,-1.8498511836927044E-05],[9.5,-1.6297291567546748E-05]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_rofrhoandft_independent_rho()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRhoAndFt\",\"independentAxis\":{\"axis\":\"rho\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"ft\",\"axisRange\":{\"start\":0,\"stop\":0.5,\"count\":11}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfRhoAndFtIndependentRho\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Rho=0.05(real)\",\"Data\":[[0.0,0.49370387899644014],[0.05,0.49370048254197518],[0.1,0.49369038714381547],[0.15000000000000002,0.49367385787945023],[0.2,0.49365128797011115],[0.25,0.49362314633916338],[0.3,0.4935899306729401],[0.35,0.49355213327477954],[0.39999999999999997,0.49351022016053664],[0.44999999999999996,0.49346462050749151],[0.49999999999999994,0.49341572298042569]]},{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Rho=0.05(imag)\",\"Data\":[[0.0,0.0],[0.05,-0.0002156004497246271],[0.1,-0.00043041182969090052],[0.15000000000000002,-0.00064371082761810266],[0.2,-0.00085488670440773767],[0.25,-0.0010634610556038866],[0.3,-0.0012690844560749052],[0.35,-0.0014715195293774563],[0.39999999999999997,-0.0016706192229483474],[0.44999999999999996,-0.0018663057591507049],[0.49999999999999994,-0.0020585527044892338]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_roffxandft_independent_ft()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            // DistributedPointSourceSDA causes an exception for this solution domain
            const string postData = "{\"forwardSolverType\":\"PointSourceSDA\",\"solutionDomain\":\"ROfFxAndFt\",\"IndependentAxis\":{\"axis\":\"ft\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0,\"stop\":0.5,\"count\":11}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfFxAndFtIndependentFt\",\"PlotList\":[{\"Label\":\"PointSourceSDA μa=0.01 μs'=1 Ft=0.05(real)\",\"Data\":[[0.0,2.4848639316800574],[0.05,2.106445955297108],[0.1,1.6823551545832978],[0.15000000000000002,1.376478415166916],[0.2,1.1529444938933089],[0.25,0.98411987619690822],[0.3,0.85281901393009019],[0.35,0.74819150993466588],[0.39999999999999997,0.66313126258618038],[0.44999999999999996,0.59281413973833241],[0.49999999999999994,0.5338595165360418]]},{\"Label\":\"PointSourceSDA μa=0.01 μs'=1 Ft=0.05(imag)\",\"Data\":[[0.0,-0.016782342167237594],[0.05,-0.006584758330568154],[0.1,-0.0027039233476860709],[0.15000000000000002,-0.0013978805916929642],[0.2,-0.00081463841701874735],[0.25,-0.00050752460955205637],[0.3,-0.00032810689361407189],[0.35,-0.00021561400849906949],[0.39999999999999997,-0.00014142147463593312],[0.44999999999999996,-9.0622166465985826E-05],[0.49999999999999994,-5.4843354150758412E-05]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_roffxandft_independent_ft_distributedpointsourcesda()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfFxAndFt\",\"independentAxis\":{\"axis\":\"ft\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0,\"stop\":0.5,\"count\":11}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfFxAndFtIndependentFt\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Ft=0.05(real)\",\"Data\":[[0.0,2.4848639316800574],[0.05,2.106445955297108],[0.1,1.6823551545832978],[0.15000000000000002,1.376478415166916],[0.2,1.1529444938933089],[0.25,0.98411987619690822],[0.3,0.85281901393009019],[0.35,0.74819150993466588],[0.39999999999999997,0.66313126258618038],[0.44999999999999996,0.59281413973833241],[0.49999999999999994,0.5338595165360418]]},{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Ft=0.05(imag)\",\"Data\":[[0.0,-0.016782342167237594],[0.05,-0.006584758330568154],[0.1,-0.0027039233476860709],[0.15000000000000002,-0.0013978805916929642],[0.2,-0.00081463841701874735],[0.25,-0.00050752460955205637],[0.3,-0.00032810689361407189],[0.35,-0.00021561400849906949],[0.39999999999999997,-0.00014142147463593312],[0.44999999999999996,-9.0622166465985826E-05],[0.49999999999999994,-5.4843354150758412E-05]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_roffxandft_independent_fx()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfFxAndFt\",\"independentAxis\":{\"axis\":\"fx\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"ft\",\"axisRange\":{\"start\":0,\"stop\":0.5,\"count\":11}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfFxAndFtIndependentFx\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Fx=0.05(real)\",\"Data\":[[0.0,2.1064852077259935],[0.05,2.106445955297108],[0.1,2.1063282416471241],[0.15000000000000002,2.1061321974931171],[0.2,2.1058580400556552],[0.25,2.1055060721036258],[0.3,2.10507668063024],[0.35,2.1045703351731477],[0.39999999999999997,2.1039875857949442],[0.44999999999999996,2.103329060743361],[0.49999999999999994,2.1025954638131492]]},{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Fx=0.05(imag)\",\"Data\":[[0.0,0.0],[0.05,-0.006584758330568154],[0.1,-0.013167456384335318],[0.15000000000000002,-0.019746039008179159],[0.2,-0.026318461260983344],[0.25,-0.0328826934317396],[0.3,-0.039436725953493963],[0.35,-0.045978574180596331],[0.39999999999999997,-0.052506282998517242],[0.44999999999999996,-0.059017931237676076],[0.49999999999999994,-0.065511635865235812]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_roffxandft_independent_fx_multi_axis()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            const string postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfFxAndFt\",\"xAxis\":{\"axis\":\"ft\",\"axisRange\":{\"start\":0,\"stop\":0.5,\"count\":11}},\"independentAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0,\"stop\":0.5,\"count\":3}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            var data = _plotSolutionDomainResultsService.Plot(solutionDomainPlotParameters);
            const string results = "{\"Id\":\"ROfFxAndFtIndependentFx\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Fx=0(real)\",\"Data\":[[0.0,2.4852397993118553],[0.05,2.4848639316800574],[0.1,2.4837439992120758],[0.15000000000000002,2.4819023819884265],[0.2,2.47937439021092],[0.25,2.4762056432443074],[0.3,2.4724490173307636],[0.35,2.4681615699748463],[0.39999999999999997,2.4634017643826094],[0.44999999999999996,2.4582271931706536],[0.49999999999999994,2.452692878129179]]},{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Fx=0(imag)\",\"Data\":[[0.0,0.0],[0.05,-0.016782342167237594],[0.1,-0.03348129061402124],[0.15000000000000002,-0.050017284416834361],[0.2,-0.0663179477113333],[0.25,-0.0823205948235818],[0.3,-0.097973706883672837],[0.35,-0.11323739231801298],[0.39999999999999997,-0.12808299270077955],[0.44999999999999996,-0.14249207444050635],[0.49999999999999994,-0.15645505893204492]]},{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Fx=0.25(real)\",\"Data\":[[0.0,0.98411996475392038],[0.05,0.98411987619690822],[0.1,0.98411961052385477],[0.15000000000000002,0.98411916772871155],[0.2,0.98411854780139618],[0.25,0.984117750727794],[0.3,0.98411677648975771],[0.35,0.9841156250651073],[0.39999999999999997,0.98411429642763038],[0.44999999999999996,0.98411279054708212],[0.49999999999999994,0.98411110738918528]]},{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Fx=0.25(imag)\",\"Data\":[[0.0,0.0],[0.05,-0.00050752460955205637],[0.1,-0.0010150500259628677],[0.15000000000000002,-0.0015225770560791407],[0.2,-0.0020301065067234764],[0.25,-0.0025376391846823267],[0.3,-0.003045175896693942],[0.35,-0.0035527174494363097],[0.39999999999999997,-0.004060264649515124],[0.44999999999999996,-0.0045678183034517025],[0.49999999999999994,-0.0050753792176709642]]},{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Fx=0.5(real)\",\"Data\":[[0.0,0.5338594418530449],[0.05,0.53385951653604169],[0.1,0.53385974058469832],[0.15000000000000002,0.53386011399801359],[0.2,0.533860636774319],[0.25,0.53386130891127925],[0.3,0.53386213040589048],[0.35,0.53386310125448244],[0.39999999999999997,0.5338642214527175],[0.44999999999999996,0.53386549099559044],[0.49999999999999994,0.53386690987742891]]},{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1 Fx=0.5(imag)\",\"Data\":[[0.0,0.0],[0.05,-5.4843354150758304E-05],[0.1,-0.00010968706195099675],[0.15000000000000002,-0.00016453147704877723],[0.2,-0.00021937695308932444],[0.25,-0.00027422384371360666],[0.3,-0.00032907250255692107],[0.35,-0.00038392328324746963],[0.39999999999999997,-0.00043877653940494507],[0.44999999999999996,-0.00049363262463911152],[0.49999999999999994,-0.000548491892548388]]}]}";
            Verify_plot_data(data, results);
        }

        [Test]
        public void Plot_solution_domain_rofrho_throws_error()
        {
            _plotSolutionDomainResultsService = new PlotSolutionDomainResultsService(_logger, new ParameterTools());
            Assert.Throws<NullReferenceException>(() =>
                _plotSolutionDomainResultsService.Plot(null));
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
