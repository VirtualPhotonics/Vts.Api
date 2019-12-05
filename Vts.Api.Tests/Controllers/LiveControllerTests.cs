using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Vts.Api.Controllers;

namespace Vts.Api.Test.Controllers
{
    class LiveControllerTests
    {
        [Test]
        public void Test_controller_get()
        {
            var liveController = new LiveController {
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
