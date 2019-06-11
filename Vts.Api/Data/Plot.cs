using System.Collections.Generic;

namespace Vts.Api.Data
{
    /// <summary>
    /// Plot data in JSON format for each plot
    /// </summary>
    public class PlotDataJson
    {
        /// <summary>
        /// Plot label
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Plot data
        /// </summary>
        public List<List<double>> Data { get; set; }
    }

    /// <summary>
    /// Plot data for each plot
    /// </summary>
    public class PlotData
    {
        /// <summary>
        /// Plot label
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        ///  Plot data
        /// </summary>
        public IEnumerable<Point> Data { get; set; }
    }
}
