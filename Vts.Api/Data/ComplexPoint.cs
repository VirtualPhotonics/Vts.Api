using System.Numerics;

namespace Vts.Api.Data
{
    /// <summary>
    /// Complex Point class with double X and Complex Y values.
    /// </summary>
    public class ComplexPoint
    {
        public double X { get; set; }
        public Complex Y { get; set; }

        /// <summary>
        /// Constructor for ComplexPoint
        /// </summary>
        /// <param name="x">x value</param>
        /// <param name="y">y value</param>
        public ComplexPoint(double x, Complex y)
        {
            X = x;
            Y = y;
        }
    }
}
