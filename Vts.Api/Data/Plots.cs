using System.Collections.Generic;

namespace Vts.Api.Data
{
    /// <summary>
    /// Plot class
    /// </summary>
    public class Plots
    {
        /// <summary>
        /// The id of the plots
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The detector used for the plots
        /// </summary>
        public string Detector { get; set; }
        /// <summary>
        /// X-axis label
        /// </summary>
        public string XAxis { get; set; }
        /// <summary>
        /// Y-axis label
        /// </summary>
        public string YAxis { get; set; }
        /// <summary>
        /// Legend label
        /// </summary>
        public string Legend { get; set; }
        /// <summary>
        /// List of plots
        /// </summary>
        public List<PlotDataJson> PlotList { get; set; }
    }

}
