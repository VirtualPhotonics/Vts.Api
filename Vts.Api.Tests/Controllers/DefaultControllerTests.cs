using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Vts.Api.Controllers;

namespace Vts.Api.Tests.Controllers
{
    internal class DefaultControllerTests
    {
        [Test]
        public void Test_controller_get()
        {
            var liveController = new DefaultController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var response = liveController.Get();
            Assert.That(liveController.HttpContext.Response.StatusCode, Is.EqualTo(200));
            Assert.That(response, Is.EqualTo("200 OK"));
        }
    }
}
