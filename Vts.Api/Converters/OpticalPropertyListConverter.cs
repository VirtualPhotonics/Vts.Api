using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vts.Api.Converters
{
    public class OpticalPropertyListConverter : JsonConverter<IEnumerable<OpticalProperties>>
    {
        public override IEnumerable<OpticalProperties> ReadJson(JsonReader reader, Type objectType, IEnumerable<OpticalProperties> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            return token.Type == JTokenType.Array
                ? token.Select(x => new OpticalProperties(
                    (double)x["mua"],
                    (double)x["musp"],
                    (double)x["g"],
                    (double)x["n"])).ToList()
                : new List<OpticalProperties>();
        }

        public override void WriteJson(JsonWriter writer, IEnumerable<OpticalProperties> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
