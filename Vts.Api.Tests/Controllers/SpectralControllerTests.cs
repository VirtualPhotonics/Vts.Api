using Moq;
using NUnit.Framework;
using Vts.Api.Controllers;
using Vts.Api.Services;

namespace Vts.Api.Tests.Controllers
{
    public class SpectralControllerTests
    {
        private Mock<ISpectralService> _spectralServiceMock;

        [OneTimeSetUp]
        public void Setup()
        {
            // set up the mock service
            _spectralServiceMock = new Mock<ISpectralService>();
        }

        [Test]
        public void Test_controller_get()
        {
            var spectralController = new SpectralController(_spectralServiceMock.Object);
            var response = spectralController.Get();
            string[] array = { "Controller", "Spectral" };
            Assert.AreEqual(array, response);
        }
    }
}
