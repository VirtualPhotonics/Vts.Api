﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.Net;
using System.Text;
using Vts.Api.Controllers;

namespace Vts.Api.Tests.Controllers
{
    internal class ReadyControllerTests
    {
        [Test]
        public void Test_controller_get()
        {
            var readyController = new ReadyController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            var response = readyController.Get();
            Assert.AreEqual("200 OK", response);
            Assert.AreEqual(200, readyController.HttpContext.Response.StatusCode);
        }

        [Test]
        public void Test_controller_post()
        {
            var readyController = new ReadyController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            const string body = "{}";
            readyController.Post(body);
            Assert.AreEqual(200, readyController.HttpContext.Response.StatusCode);
        }

        [Test]
        public async Task Test_post_with_api_key()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var httpClient = server.CreateClient();
            const string url = "api/ready";
            const HttpStatusCode expected = HttpStatusCode.OK;
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", "TESTKEY");
            var response = await httpClient.PostAsync(url, new StringContent("{}", Encoding.UTF8, "application/json"));
            Assert.AreEqual(expected, response.StatusCode);
        }

        [Test]
        public void Test_post_with_api_key_unauthorized()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var client = server.CreateClient();
            const string url = "api/ready";
            const HttpStatusCode expected = HttpStatusCode.Unauthorized;

            var response = client.PostAsync(url, new StringContent("{}", Encoding.UTF8, "application/json"));

            Assert.AreEqual(expected, response.Result.StatusCode);
        }
    }
}
