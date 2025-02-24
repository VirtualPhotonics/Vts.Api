using NUnit.Framework;
using System.Numerics;
using Vts.Api.Data;

namespace Vts.Api.Tests.Data
{
    internal class ComplexPointTests
    {
        [Test]
        public void Test_complex_point()
        {
            var point = new ComplexPoint(0.4, new Complex(0.1, 0.2));
            Assert.That(point.Y.Real, Is.EqualTo(0.1));
            Assert.That(point.Y.Imaginary, Is.EqualTo(0.2));
            Assert.That(point.X, Is.EqualTo(0.4));
        }
    }
}
