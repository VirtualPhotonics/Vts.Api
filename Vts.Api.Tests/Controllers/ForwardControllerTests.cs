using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Vts.Api.Controllers;
using Vts.Api.Data;
using Vts.Api.Models;
using Vts.Api.Services;

namespace Vts.Api.Tests.Controllers
{
    internal class ForwardControllerTests
    {
        private Mock<IForwardSolverService> _forwardSolverServiceMock;

        [OneTimeSetUp]
        public void Setup()
        {
            // set up the mock service
            _forwardSolverServiceMock = new Mock<IForwardSolverService>();
        }

        [Test]
        public void Test_controller_get()
        {
            var forwardController = new ForwardController(_forwardSolverServiceMock.Object);
            var response = forwardController.Get();
            string[] array = { "Controller", "Forward" };
            Assert.AreEqual(array, response);
        }

        [Test]
        public void Test_controller_post()
        {
            const string results = "{\"Id\":\"ROfRho\",\"PlotList\":[{\"Label\":\"DistributedPointSourceSDA μa=0.01 μs'=1\",\"Data\":[[0.5,0.046625597794768131],[1.0,0.020915537635778064],[1.5,0.012316866807976681],[2.0,0.0080842309517170675],[2.5,0.005623379122941038],[3.0,0.0040562789854642293],[3.5,0.0030006454971476613],[4.0,0.0022621931768063925],[4.5,0.0017313878247261138],[5.0,0.001341834630580925],[5.5,0.00105114060787151],[6.0,0.00083118079655800016],[6.5,0.00066274418610378413],[7.0,0.00053240356107749358],[7.5,0.00043059663530889934],[8.0,0.00035040528102366],[8.5,0.00028675569244185416],[9.0,0.000235881710029713],[9.5,0.00019495753939373391]]}]}";
            var expected = JsonConvert.DeserializeObject<Plots>(results);
            _forwardSolverServiceMock.Setup(x => x.GetPlotData(It.IsAny<SolutionDomainPlotParameters>()))
                .Returns(expected);
            var forwardController = new ForwardController(_forwardSolverServiceMock.Object);
            var response = forwardController.Post(new SolutionDomainPlotParameters());
            Assert.IsInstanceOf<Plots>(response);
        }
    }
}
