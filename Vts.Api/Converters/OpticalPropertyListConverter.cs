using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vts.Api.Converters
{
    /// <summary>
    /// Class to convert a JSON object into an enumerable of OpticalProperties 
    /// </summary>
    public class OpticalPropertyListConverter : JsonConverter<IEnumerable<OpticalProperties>>
    {
        /// <summary>
        /// Reads the JSON object and returns a list of OpticalProperties 
        /// </summary>
        /// <param name="reader">JSON reader</param>
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">The existing value</param>
        /// <param name="hasExistingValue">Boolean for existing value</param>
        /// <param name="serializer">JSON serializer</param>
        /// <returns>A list of OpticalProperties</returns>
        public override IEnumerable<OpticalProperties> ReadJson(JsonReader reader, Type objectType, IEnumerable<OpticalProperties> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            try
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
            catch (Exception)
            {
                return new List<OpticalProperties>();
            }
        }

        public override void WriteJson(JsonWriter writer, IEnumerable<OpticalProperties> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
