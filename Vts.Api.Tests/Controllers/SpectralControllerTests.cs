using Moq;
using NUnit.Framework;
using Vts.Api.Controllers;
using Vts.Api.Services;

namespace Vts.Api.Test.Controllers
{
    public class SpectralControllerTests
    {
        private Mock<ISpectralService> spectralServiceMock;

        [OneTimeSetUp]
        public void Setup()
        {
            // set up the mock service
            spectralServiceMock = new Mock<ISpectralService>();
        }

        [Test]
        public void Test_controller_get()
        {
            var spectralController = new SpectralController(spectralServiceMock.Object);
            var response = spectralController.Get();
            string[] array = { "Controller", "Spectral" };
            Assert.AreEqual(array, response);
        }
    }
}
