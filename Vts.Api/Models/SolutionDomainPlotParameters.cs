using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Vts.Common;

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
        public DoubleRange XAxis { get; set; }
        public OpticalProperties OpticalProperties { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OptimizerType OptimizerType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public InverseFitType OptimizationParameters { get; set; }

        public LabelValuePair IndependentAxes { get; set; }
        public double NoiseValue { get; set; }
        public string ModelAnalysis { get; set; }
        public List<double[]> MeasuredData { get; set; }
    }
}
