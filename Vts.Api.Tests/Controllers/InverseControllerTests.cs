using Moq;
using NUnit.Framework;
using Vts.Api.Controllers;
using Vts.Api.Services;

namespace Vts.Api.Tests.Controllers
{
    public class InverseControllerTests
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
        
    }
}
