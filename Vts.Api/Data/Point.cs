namespace Vts.Api.Data
{
    /// <summary>
    /// Point class with double X and Y values.
    /// </summary>
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        /// <summary>
        /// Constructor for Point
        /// </summary>
        /// <param name="x">x value</param>
        /// <param name="y">y value</param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}