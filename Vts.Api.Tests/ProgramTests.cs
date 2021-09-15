using NUnit.Framework;

namespace Vts.Api.Tests
{
    internal class ProgramTests
    {
        [Test]
        public void Test_create_web_host_builder()
        {
            string[] args = { };
            var webhost = Program.CreateWebHostBuilder(args);
            Assert.IsNotNull(webhost);
        }
    }
}
