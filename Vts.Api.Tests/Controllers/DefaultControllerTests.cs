using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Vts.Api.Controllers;

namespace Vts.Api.Tests.Controllers
{
    class DefaultControllerTests
    {
        [Test]
        public void Test_controller_get()
        {
            var liveController = new DefaultController {
                ControllerContext = new ControllerContext {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var response = liveController.Get();
            Assert.AreEqual(200, liveController.HttpContext.Response.StatusCode);
            Assert.AreEqual("200 OK", response);
        }
    }
}
