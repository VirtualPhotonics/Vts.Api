using Newtonsoft.Json;
using NUnit.Framework;
using Vts.Api.Controllers;
using Vts.Api.Data;
using Vts.Api.Models;
using Vts.Api.Services;

namespace Vts.Api.Tests.Controllers
{
    internal class SpectralControllerTests
    {
        private ISpectralService _spectralServiceMock;
        [OneTimeSetUp]
        public void Setup()
        {
            // set up the mock service
            _spectralServiceMock = Substitute.For<ISpectralService>();
        }

        [Test]
        public void Test_controller_get()
        {
            var spectralController = new SpectralController(_spectralServiceMock);
            var response = spectralController.Get();
            string[] array =
            {
                "Controller",
                "Spectral"
            };
            Assert.AreEqual(array, response);
        }

        [Test]
        public void Test_controller_post()
        {
            const string results = "{\"id\":\"SpectralMusp\",\"plotList\":[{\"label\":\"Skin μs'\",\"data\":[[650,2.2123013258570863],[700,1.991324464076181],[750,1.8054865273011322],[800,1.6473787686682118],[850,1.5114938050444233],[900,1.3936601517386393],[950,1.290665574863872],[1000,1.2]]}]}";
            var expected = JsonConvert.DeserializeObject<Plots>(results);
            _spectralServiceMock.GetPlotData(Arg.Any<SpectralPlotParameters>()).Returns(expected);
            var spectralController = new SpectralController(_spectralServiceMock);
            var response = spectralController.Post(new SpectralPlotParameters());
            Assert.IsInstanceOf<Plots>(response);
        }
    }
}