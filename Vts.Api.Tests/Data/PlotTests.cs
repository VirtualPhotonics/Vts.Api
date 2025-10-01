using NUnit.Framework;
using Vts.Api.Data;

namespace Vts.Api.Tests.Data
{
    internal class PlotTests
    {
        [Test]
        public void Test_complex_point()
        {
            var plotData = new PlotData
            {
                Label = "label",
                Data = new List<Point>
                {
                    new Point(1,2),
                    new Point(3,4)
                }
            };
            Assert.That(plotData.Label, Is.EqualTo("label"));
            Assert.That(plotData.Data.ElementAt(0).X, Is.EqualTo(1));
            Assert.That(plotData.Data.ElementAt(0).Y, Is.EqualTo(2));
            Assert.That(plotData.Data.ElementAt(1).X, Is.EqualTo(3));
            Assert.That(plotData.Data.ElementAt(1).Y, Is.EqualTo(4));
        }
    }
}
