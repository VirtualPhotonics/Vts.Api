using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Vts.Api.Enums;
using Vts.Common;
using Vts.SpectralMapping;

namespace Vts.Api.Models
{
    public class SpectralPlotParameters : IPlotParameters
    {
        /// <summary>
        /// x-axis is wavelength
        /// </summary>
        public DoubleRange XAxis { get; set; }
        
        /// <summary>
        /// y-axis
        /// </summary>
        public string YAxis {  get; set; }
        
        /// <summary>
        /// Spectral plot type Enum
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public SpectralPlotType SpectralPlotType { get; set; }

        public string PlotName { get; set; }
        public Tissue Tissue { get; set; }
        public double[] Wavelengths { get; set; }
        public string TissueType { get; set; }

        /// <summary>
        /// The scatterer type Enum
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ScatteringType ScatteringType { get; set; }
        public List<LabelValuePair> AbsorberConcentration { get; set; }
        public PowerLawScatterer PowerLawScatterer { get; set; }
        public IntralipidScatterer IntralipidScatterer { get; set; }
        public MieScatterer MieScatterer { get; set; }
    }
}
