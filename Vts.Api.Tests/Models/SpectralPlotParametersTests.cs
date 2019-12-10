using Newtonsoft.Json;
using NUnit.Framework;
using Vts.Api.Enums;
using Vts.Api.Models;
using Vts.Common;
using Vts.SpectralMapping;

namespace Vts.Api.Tests.Models
{
    class SpectralPlotParametersTests
    {
        [Test]
        public void Test_deserialize_spectral_data()
        {
            var postData = "{\"spectralPlotType\":\"musp\",\"plotName\":\"μs'\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638,\"visible\":true},\"scatteringType\":\"PowerLaw\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42,\"show\":true},\"intralipidScatterer\":{\"volumeFraction\":0.01,\"show\":false},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01,\"show\":false},\"xAxis\":{\"title\":\"Wavelength Range\",\"startLabel\":\"Begin\",\"startLabelUnits\":\"nm\",\"start\":650,\"endLabel\":\"End\",\"endLabelUnits\":\"nm\",\"stop\":1000,\"numberLabel\":\"Number\",\"count\":36}}";
            var xAxis = new DoubleRange(650, 1000, 36);
            var powerLawScatterer = new PowerLawScatterer(1.2, 1.42);
            var intralipidScatterer = new IntralipidScatterer(0.01);
            var mieScatterer = new MieScatterer(0.5, 1.4, 1, 0.01);
            var absorberConcentrations = new[] {new LabelValuePair() {Label = "Hb", Value = 28.4}};
            var spectralPlotParameters = JsonConvert.DeserializeObject<SpectralPlotParameters>(postData);
            Assert.AreEqual(xAxis.Start, spectralPlotParameters.XAxis.Start);
            Assert.AreEqual(xAxis.Stop, spectralPlotParameters.XAxis.Stop);
            Assert.AreEqual(xAxis.Count, spectralPlotParameters.XAxis.Count);
            Assert.AreEqual(xAxis.Delta, spectralPlotParameters.XAxis.Delta);
            Assert.IsNull(spectralPlotParameters.YAxis);
            Assert.AreEqual(SpectralPlotType.Musp, spectralPlotParameters.SpectralPlotType);
            Assert.AreEqual("μs'", spectralPlotParameters.PlotName);
            Assert.IsNull(spectralPlotParameters.Tissue);
            Assert.IsNull(spectralPlotParameters.Wavelengths);
            Assert.AreEqual("Skin", spectralPlotParameters.TissueType);
            Assert.AreEqual(ScatteringType.PowerLaw, spectralPlotParameters.ScatteringType);
            Assert.AreEqual(absorberConcentrations[0].Label, spectralPlotParameters.AbsorberConcentration[0].Label);
            Assert.AreEqual(absorberConcentrations[0].Value, spectralPlotParameters.AbsorberConcentration[0].Value);
            Assert.AreEqual(powerLawScatterer.A, spectralPlotParameters.PowerLawScatterer.A);
            Assert.AreEqual(intralipidScatterer.VolumeFraction, spectralPlotParameters.IntralipidScatterer.VolumeFraction);
            Assert.AreEqual(mieScatterer.ParticleRadius, spectralPlotParameters.MieScatterer.ParticleRadius);
        }
    }
}
