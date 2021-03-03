using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Vts.Api.Controllers;
using Vts.Api.Data;
using Vts.Api.Models;
using Vts.Api.Services;

namespace Vts.Api.Tests.Controllers
{
    internal class InverseControllerTests
    {
        private Mock<IInverseSolverService> _inverseSolverMock;

        [OneTimeSetUp]
        public void Setup()
        {
            // set up the mock service
            _inverseSolverMock = new Mock<IInverseSolverService>();
        }

        [Test]
        public void Test_controller_get()
        {
            var inverseController = new InverseController(_inverseSolverMock.Object);
            var response = inverseController.Get();
            string[] array = { "Controller", "Inverse" };
            Assert.AreEqual(array, response);
        }

        [Test]
        public void Test_controller_post()
        {
            const string results = "{\"id\":\"ROfRho\",\"plotList\":[{\"label\":\"PointSourceSDA μa=0 μs'=0.900815675674306 Time=0.05\",\"data\":[[0.5,0.03189082987207784],[1.625,0.012632584536081196],[2.75,0.00598596460065667],[3.875,0.0033409123998315196],[5,0.002044773826752982],[6.125,0.0013303379558068488],[7.25,0.0009063429452912182],[8.375,0.0006408987656963273],[9.5,0.00046750924657831995]]}]}";
            var expected = JsonConvert.DeserializeObject<Plots>(results);
            _inverseSolverMock.Setup(x => x.GetPlotData(It.IsAny<SolutionDomainPlotParameters>()))
                .Returns(expected);
            var spectralController = new InverseController(_inverseSolverMock.Object);
            var response = spectralController.Post(new SolutionDomainPlotParameters());
            Assert.IsInstanceOf<Plots>(response);
        }
    }
}
