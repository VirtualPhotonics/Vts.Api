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
        /// List of plots
        /// </summary>
        public List<PlotDataJson> PlotList { get; set; }
    }

}