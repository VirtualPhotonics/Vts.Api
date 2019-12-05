using Moq;
using NUnit.Framework;
using Vts.Api.Controllers;
using Vts.Api.Services;

namespace Vts.Api.Tests.Controllers
{
    public class ForwardControllerTests
    {
        private Mock<IForwardSolverService> forwardSolverMock;

        [OneTimeSetUp]
        public void Setup()
        {
            // set up the mock service
            forwardSolverMock = new Mock<IForwardSolverService>();
        }

        [Test]
        public void Test_controller_get()
        {
            var forwardController = new ForwardController(forwardSolverMock.Object);
            var response = forwardController.Get();
            string[] array = { "Controller", "Forward" };
            Assert.AreEqual(array, response);
        }
    }
}
