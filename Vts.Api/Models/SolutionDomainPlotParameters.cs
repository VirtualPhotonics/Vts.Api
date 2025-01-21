using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Vts.Api.Converters;

namespace Vts.Api.Models
{
    public class SolutionDomainPlotParameters : IPlotParameters
    {
        [JsonRequired]
        [JsonConverter(typeof(StringEnumConverter))]
        public ForwardSolverType ForwardSolverType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ForwardSolverType InverseSolverType { get; set; }

        [JsonRequired]
        [JsonConverter(typeof(StringEnumConverter))]
        public SolutionDomainType SolutionDomain { get; set; }
        [JsonRequired]
        public IndependentAxis XAxis { get; set; }
        public OpticalProperties OpticalProperties { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OptimizerType OptimizerType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public InverseFitType OptimizationParameters { get; set; }
        public double NoiseValue { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ForwardAnalysisType ModelAnalysis { get; set; }
        public List<double[]> MeasuredData { get; set; }
        [JsonConverter(typeof(OpticalPropertyListConverter))]
        public List<OpticalProperties> WavelengthOpticalPropertyList { get; set; }
        public IndependentAxis IndependentAxis { get; set; }
        public IndependentAxis SecondIndependentAxis { get; set; }

        public string ValidateData()
        {
            if (OpticalProperties == null && WavelengthOpticalPropertyList.Count == 0)
            {
                return "{\"Error\": \"OpticalProperties or WavelengthOpticalPropertyList must have a value\"}";
            }

            if (XAxis.Axis == IndependentVariableAxis.Wavelength && WavelengthOpticalPropertyList.Count != XAxis.AxisRange.Count)
            {
                return $"{{\"Error\": \"The WavelengthOpticalPropertyList (count: {WavelengthOpticalPropertyList.Count}) must have the same number of values as the axis range (count: {XAxis.AxisRange.Count})\"}}";
            }
            return "";
        }
    }
}