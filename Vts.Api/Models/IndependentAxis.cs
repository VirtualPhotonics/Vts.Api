using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Vts.Common;

namespace Vts.Api.Models
{
    public class IndependentAxis
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public IndependentVariableAxis Axis { get; set; }
        public double AxisValue { get; set; }
        public DoubleRange AxisRange { get; set; }
    }
}