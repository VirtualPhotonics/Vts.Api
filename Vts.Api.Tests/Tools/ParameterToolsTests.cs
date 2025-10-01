using NUnit.Framework;
using Vts.Api.Tools;

namespace Vts.Api.Tests.Tools
{
    internal class ParameterToolsTests
    {
        private IParameterTools _parameterTools;

        [Test]
        public void Test_get_optical_properties_null()
        {
            _parameterTools = new ParameterTools();
            var opticalProperties = _parameterTools.GetOpticalPropertiesObject(null, null);
            Assert.That(opticalProperties, Is.InstanceOf<List<OpticalProperties>>());
        }
    }
}
