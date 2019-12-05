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
    class InverseSolverServiceTests
    {
        private InverseSolverService _inverseSolverService;
        private ILoggerFactory _factory;
        private ILogger<InverseSolverService> _logger;
        private Mock<IPlotFactory> _plotFactoryMock;

        [Test]
        public void Test_get_plot_data()
        {
            _plotFactoryMock = new Mock<IPlotFactory>();
            _plotFactoryMock.Setup(x => x.GetPlot(PlotType.SolutionDomain, new SolutionDomainPlotParameters())).Returns("");
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            _factory = serviceProvider.GetService<ILoggerFactory>()
                .AddConsole();
            _logger = _factory.CreateLogger<InverseSolverService>();
            _inverseSolverService = new InverseSolverService(_logger, _plotFactoryMock.Object);
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRho\",\"measuredData\":[[0.5,0.034828428710495074],[0.7571428571428571,0.029420531983087726],[1.0142857142857142,0.02093360911749075],[1.2714285714285714,0.01665715182769329],[1.5285714285714285,0.014364183918250022],[1.7857142857142856,0.011454828142680954],[2.0428571428571427,0.009079166450343084],[2.3,0.00766922059364649],[2.557142857142857,0.005969062618866233],[2.814285714285714,0.005133772121534473],[3.071428571428571,0.004173573203610217],[3.3285714285714283,0.0037753248321977808],[3.5857142857142854,0.0033053865379579577],[3.8428571428571425,0.0025773098635545667],[4.1,0.00232888357135617],[4.357142857142857,0.001995707298051381],[4.614285714285714,0.0016130076959352349],[4.871428571428571,0.0015272034490867183],[5.128571428571428,0.0013992385402248396],[5.385714285714285,0.0011945241210807522],[5.642857142857142,0.0010810369822821084],[5.8999999999999995,0.0009448803801525864],[6.157142857142857,0.0009198316127711655],[6.414285714285714,0.0006943417260433173],[6.671428571428571,0.0006333583166048397],[6.928571428571428,0.0006216988605675773],[7.185714285714285,0.000512835051348594],[7.442857142857142,0.0004826500754318199],[7.699999999999999,0.00043617857542623087],[7.957142857142856,0.0003878849790650086],[8.214285714285714,0.000378880207382442],[8.471428571428572,0.00031667904143385504],[8.728571428571428,0.00026887511680669254],[8.985714285714284,0.00025470774216120143],[9.24285714285714,0.00022926547222261112],[9.499999999999996,0.000201735054811538]],\"independentAxes\":{\"show\":false,\"first\":\"ρ\",\"second\":\"t\",\"label\":\"t\",\"value\":0.05,\"units\":\"ns\",\"firstUnits\":\"mm\",\"secondUnits\":\"ns\"},\"xAxis\":{\"title\":\"Detector Positions\",\"startLabel\":\"Begin\",\"startLabelUnits\":\"mm\",\"start\":0.5,\"endLabel\":\"End\",\"endLabelUnits\":\"mm\",\"stop\":9.5,\"numberLabel\":\"Number\",\"count\":\"36\"},\"opticalProperties\":{\"title\":\"Initial Guess Optical Properties\",\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var results = _inverseSolverService.GetPlotData(JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData));
            Assert.IsNull(results);
        }
    }
}
