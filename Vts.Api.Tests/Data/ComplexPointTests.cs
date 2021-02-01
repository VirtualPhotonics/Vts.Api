using System.Numerics;
using NUnit.Framework;
using Vts.Api.Data;

namespace Vts.Api.Tests.Data
{
    internal class ComplexPointTests
    {
        [Test]
        public void Test_complex_point()
        {
            var point = new ComplexPoint(0.4, new Complex(0.1, 0.2));
            Assert.AreEqual(0.1, point.Y.Real);
            Assert.AreEqual(0.2, point.Y.Imaginary);
            Assert.AreEqual(0.4, point.X);
        }
    }
}
