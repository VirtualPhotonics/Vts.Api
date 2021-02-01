using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace Vts.Api.Tests.Converters
{
    internal class OpticalPropertyListConverterTests
    {

        [Test]
        public void Test_write_json_throws_error()
        {
            var converter = new OpticalPropertiesConverter();
            Assert.Throws<NotImplementedException>(() =>
                converter.WriteJson(new JsonTextWriter(new StringWriter(new StringBuilder())), null,
                    JsonSerializer.CreateDefault()));
        }
    }
}
