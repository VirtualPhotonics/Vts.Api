using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vts.SpectralMapping;

namespace Vts.Api.Converters
{
    public class ScattererConverter: JsonConverter<IScatterer>
    {
        public override IScatterer ReadJson(JsonReader reader, Type objectType, IScatterer existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            IScatterer scatterer;
            double volumeFraction;
            // Load the JSON for the scatterer into a JObject
            var jsonObject = JObject.Load(reader);
            var currentToken = reader.Path;
            ScatteringType scatteringType;
            switch (existingValue)
            {
                case PowerLawScatterer _:
                    scatteringType = ScatteringType.PowerLaw;
                    break;
                case IntralipidScatterer _:
                    scatteringType = ScatteringType.Intralipid;
                    break;
                case MieScatterer _:
                    scatteringType = ScatteringType.Mie;
                    break;
                default:
                    var scattererEnum = currentToken.Replace("Scatterer", "");
                    Enum.TryParse<ScatteringType>(scattererEnum, true, out scatteringType);
                    break;
            }
            switch (scatteringType)
            {
                case ScatteringType.Intralipid:
                    if (jsonObject["volumeFraction"] != null)
                    {
                        volumeFraction = jsonObject["volumeFraction"].Value<double>();
                        scatterer = new IntralipidScatterer(volumeFraction);
                    }
                    else
                    {
                        scatterer = new IntralipidScatterer();
                    }
                    break;
                case ScatteringType.Mie:
                    if (jsonObject["particleRadius"] != null && jsonObject["particleRefractiveIndex"] != null && jsonObject["mediumRefractiveIndex"] != null && jsonObject["volumeFraction"] != null)
                    {
                        var particleRadius = jsonObject["particleRadius"].Value<double>();
                        var particleRefractiveIndex = jsonObject["particleRefractiveIndex"].Value<double>();
                        var mediumRefractiveIndex = jsonObject["mediumRefractiveIndex"].Value<double>();
                        volumeFraction = jsonObject["volumeFraction"].Value<double>();
                        scatterer = new MieScatterer(particleRadius, particleRefractiveIndex, mediumRefractiveIndex, volumeFraction);
                    }
                    else
                    {
                        scatterer = new MieScatterer();
                    }
                    break;
                case ScatteringType.PowerLaw:
                    if (jsonObject["a"] != null && jsonObject["b"] != null)
                    {
                        var a = jsonObject["a"].Value<double>();
                        var b = jsonObject["b"].Value<double>();
                        scatterer = new PowerLawScatterer(a, b);
                    }
                    else
                    {
                        scatterer = new PowerLawScatterer();
                    }
                    break;
                default:
                    scatterer = new PowerLawScatterer();
                    break;
            }
            // Return the scatterer
            return scatterer;
        }

        public override void WriteJson(JsonWriter writer, IScatterer value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
