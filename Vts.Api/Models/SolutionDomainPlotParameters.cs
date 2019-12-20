using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Vts.Api.Models
{
    public class SolutionDomainPlotParameters : IPlotParameters
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ForwardSolverType ForwardSolverType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ForwardSolverType InverseSolverType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public SolutionDomainType SolutionDomain { get; set; }
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
        public List<OpticalProperties> WavelengthOpticalPropertyList { get; set; }
        public IndependentAxis IndependentAxis { get; set; }
        public IndependentAxis SecondIndependentAxis { get; set; }
    }
}