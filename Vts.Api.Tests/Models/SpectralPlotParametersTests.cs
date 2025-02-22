using Newtonsoft.Json;
using NUnit.Framework;
using Vts.Api.Enums;
using Vts.Api.Models;
using Vts.Common;
using Vts.SpectralMapping;

namespace Vts.Api.Tests.Models
{
    internal class SpectralPlotParametersTests
    {
        [Test]
        public void Test_deserialize_spectral_data()
        {
            const string postData = "{\"spectralPlotType\":\"musp\",\"plotName\":\"μs'\",\"tissueType\":\"Skin\",\"absorberConcentration\":[{\"label\":\"Hb\",\"value\":28.4,\"units\":\"μM\"},{\"label\":\"HbO2\",\"value\":22.4,\"units\":\"μM\"},{\"label\":\"H2O\",\"value\":0.7,\"units\":\"vol. frac.\"},{\"label\":\"Fat\",\"value\":0,\"units\":\"vol. frac.\"},{\"label\":\"Melanin\",\"value\":0.0051,\"units\":\"vol. frac.\"}],\"bloodConcentration\":{\"totalHb\":50.8,\"bloodVolume\":0.021844,\"stO2\":0.4409448818897638},\"scatteringType\":\"PowerLaw\",\"powerLawScatterer\":{\"a\":1.2,\"b\":1.42},\"intralipidScatterer\":{\"volumeFraction\":0.01},\"mieScatterer\":{\"particleRadius\":0.5,\"ParticleRefractiveIndexMismatch\":1.4,\"MediumRefractiveIndexMismatch\":1,\"volumeFraction\":0.01},\"xAxis\":{\"axis\":\"wavelength\",\"axisRange\":{\"start\":650,\"stop\":1000,\"count\":36}}}";
            var xAxis = new DoubleRange(650, 1000, 36);
            var powerLawScatterer = new PowerLawScatterer(1.2, 1.42);
            var intralipidScatterer = new IntralipidScatterer(0.01);
            var mieScatterer = new MieScatterer(0.5, 1.4, 1, 0.01);
            var absorberConcentrations = new[] { new LabelValuePair() { Label = "Hb", Value = 28.4 } };
            var spectralPlotParameters = JsonConvert.DeserializeObject<SpectralPlotParameters>(postData);
            Assert.That(spectralPlotParameters.XAxis.AxisRange.Start, Is.EqualTo(xAxis.Start));
            Assert.That(spectralPlotParameters.XAxis.AxisRange.Stop, Is.EqualTo(xAxis.Stop));
            Assert.That(spectralPlotParameters.XAxis.AxisRange.Count, Is.EqualTo(xAxis.Count));
            Assert.That(spectralPlotParameters.XAxis.AxisRange.Delta, Is.EqualTo(xAxis.Delta));
            Assert.That(spectralPlotParameters.YAxis, Is.Null);
            Assert.That(spectralPlotParameters.SpectralPlotType, Is.EqualTo(SpectralPlotType.Musp));
            Assert.That(spectralPlotParameters.PlotName, Is.EqualTo("μs'"));
            Assert.That(spectralPlotParameters.Tissue, Is.Null);
            Assert.That(spectralPlotParameters.Wavelengths, Is.Null);
            Assert.That(spectralPlotParameters.TissueType, Is.EqualTo("Skin"));
            Assert.That(spectralPlotParameters.ScatteringType, Is.EqualTo(ScatteringType.PowerLaw));
            Assert.That(spectralPlotParameters.AbsorberConcentration[0].Label, Is.EqualTo(absorberConcentrations[0].Label));
            Assert.That(spectralPlotParameters.AbsorberConcentration[0].Value, Is.EqualTo(absorberConcentrations[0].Value));
            Assert.That(spectralPlotParameters.PowerLawScatterer.A, Is.EqualTo(powerLawScatterer.A));
            Assert.That(spectralPlotParameters.IntralipidScatterer.VolumeFraction, Is.EqualTo(intralipidScatterer.VolumeFraction));
            Assert.That(spectralPlotParameters.MieScatterer.ParticleRadius, Is.EqualTo(mieScatterer.ParticleRadius));
        }
    }
}
