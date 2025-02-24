using Newtonsoft.Json;
using NUnit.Framework;
using System.Text;
using Vts.Api.Converters;
using Vts.SpectralMapping;

namespace Vts.Api.Tests.Converters
{
    internal class ScattererConverterTests
    {
        [Test]
        public void Test_powerlaw_scatterer_json_deserializer()
        {
            const string json = "{\"a\": 1.2,\"b\": 1.42}";
            var reader = new JsonTextReader(new StringReader(json));
            while (reader.TokenType == JsonToken.None)
            {
                if (!reader.Read())
                    break;

                var converter = new ScattererConverter();
                var obj = (PowerLawScatterer)converter.ReadJson(reader, typeof(PowerLawScatterer), new PowerLawScatterer(), false, JsonSerializer.CreateDefault());
                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.A, Is.EqualTo(1.2));
                Assert.That(obj.B, Is.EqualTo(1.42));
            }
        }

        [Test]
        public void Test_powerlawscatterer_default_json_deserializer()
        {
            const string json = "{\"powerLawScatterer\": {}}";
            var reader = new JsonTextReader(new StringReader(json));
            while (reader.TokenType == JsonToken.None)
            {
                if (!reader.Read())
                    break;
                var converter = new ScattererConverter();
                var obj = (PowerLawScatterer)converter.ReadJson(reader, typeof(PowerLawScatterer), new PowerLawScatterer(), false, JsonSerializer.CreateDefault());
                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.A, Is.EqualTo(1.0));
                Assert.That(obj.B, Is.EqualTo(0.1));
            }
        }

        [Test]
        public void Test_intralipid_scatterer_json_deserializer()
        {
            const string json = "{\"volumeFraction\": 0.02}";
            var reader = new JsonTextReader(new StringReader(json));
            while (reader.TokenType == JsonToken.None)
            {
                if (!reader.Read())
                    break;
                var converter = new ScattererConverter();
                var obj = (IntralipidScatterer)converter.ReadJson(reader, typeof(IntralipidScatterer), new IntralipidScatterer(), false, JsonSerializer.CreateDefault());
                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.VolumeFraction, Is.EqualTo(0.02));
            }
        }

        [Test]
        public void Test_intralipid_scatterer_default_json_deserializer()
        {
            const string json = "{\"intralipidScatterer\": {}}";
            var reader = new JsonTextReader(new StringReader(json));
            while (reader.TokenType == JsonToken.None)
            {
                if (!reader.Read())
                    break;
                var converter = new ScattererConverter();
                var obj = (IntralipidScatterer)converter.ReadJson(reader, typeof(IntralipidScatterer), new IntralipidScatterer(), false, JsonSerializer.CreateDefault());
                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.VolumeFraction, Is.EqualTo(0.01));
            }
        }

        [Test]
        public void Test_mie_scatterer_json_deserializer()
        {
            const string json = "{\"particleRadius\": 0.8,\"particleRefractiveIndex\": 1.6,\"mediumRefractiveIndex\": 1.1,\"volumeFraction\": 0.1}";
            var reader = new JsonTextReader(new StringReader(json));
            while (reader.TokenType == JsonToken.None)
            {
                if (!reader.Read())
                    break;

                var converter = new ScattererConverter();
                var obj = (MieScatterer)converter.ReadJson(reader, typeof(MieScatterer), new MieScatterer(), false, JsonSerializer.CreateDefault());
                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.ParticleRadius, Is.EqualTo(0.8));
                Assert.That(obj.ParticleRefractiveIndexMismatch, Is.EqualTo(1.6));
                Assert.That(obj.MediumRefractiveIndexMismatch, Is.EqualTo(1.1));
                Assert.That(obj.VolumeFraction, Is.EqualTo(0.1));
            }
        }

        [Test]
        public void Test_mie_scatterer_default_json_deserializer()
        {
            const string json = "{\"mieScatterer\": {}}";
            var reader = new JsonTextReader(new StringReader(json));
            while (reader.TokenType == JsonToken.None)
            {
                if (!reader.Read())
                    break;
                var converter = new ScattererConverter();
                var obj = (MieScatterer)converter.ReadJson(reader, typeof(MieScatterer), new MieScatterer(), false, JsonSerializer.CreateDefault());
                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.ParticleRadius, Is.EqualTo(0.5));
                Assert.That(obj.ParticleRefractiveIndexMismatch, Is.EqualTo(1.4));
                Assert.That(obj.MediumRefractiveIndexMismatch, Is.EqualTo(1.0));
                Assert.That(obj.VolumeFraction, Is.EqualTo(0.01));
            }
        }

        [Test]
        public void Test_invalidscatterer_json_deserializer()
        {
            const string json = "{\"this\": \"that\"}";
            var reader = new JsonTextReader(new StringReader(json));
            while (reader.TokenType == JsonToken.None)
            {
                if (!reader.Read())
                    break;

                var converter = new ScattererConverter();
                var obj = converter.ReadJson(reader, null, null, false, JsonSerializer.CreateDefault());
                Assert.That(obj is PowerLawScatterer, Is.True);
            }
        }

        [Test]
        public void Test_write_json_throws_error()
        {
            var converter = new ScattererConverter();
            Assert.Throws<NotImplementedException>(() =>
                converter.WriteJson(new JsonTextWriter(new StringWriter(new StringBuilder())), null,
                    JsonSerializer.CreateDefault()));
        }
    }
}
