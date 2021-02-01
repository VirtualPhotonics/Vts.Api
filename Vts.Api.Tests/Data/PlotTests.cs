using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Assert.AreEqual("label", plotData.Label);
            Assert.AreEqual(1, plotData.Data.ElementAt(0).X);
            Assert.AreEqual(2, plotData.Data.ElementAt(0).Y);
            Assert.AreEqual(3, plotData.Data.ElementAt(1).X);
            Assert.AreEqual(4, plotData.Data.ElementAt(1).Y);
        }
    }
}
