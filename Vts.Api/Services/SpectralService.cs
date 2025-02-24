using Vts.Api.Data;
using Vts.Api.Enums;
using Vts.Api.Factories;
using Vts.Api.Models;
using Vts.SpectralMapping;

namespace Vts.Api.Services
{
    public class SpectralService : ISpectralService
    {
        private readonly ILogger<SpectralService> _logger;
        private readonly IPlotFactory _plotFactory;

        public SpectralService(ILogger<SpectralService> logger, IPlotFactory plotFactory)
        {
            _logger = logger;
            _plotFactory = plotFactory;
        }

        public Plots GetPlotData(SpectralPlotParameters plotParameters)
        {
            _logger.LogInformation("Get the plot data for the Spectral Panel");

            plotParameters.YAxis = plotParameters.PlotName;

            // set up the absorber concentrations
            var chromophoreAbsorbers = new List<IChromophoreAbsorber>();
            if (plotParameters.AbsorberConcentration == null)
            {
                if (Enum.TryParse(plotParameters.TissueType, out TissueType tissueType))
                {
                    chromophoreAbsorbers = TissueProvider.CreateAbsorbers(tissueType).ToList();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                chromophoreAbsorbers.AddRange(plotParameters.AbsorberConcentration.Select(absorber => new ChromophoreAbsorber(Enum.Parse<ChromophoreType>(absorber.Label, true), absorber.Value)));
            }

            // set up the scatterer
            IScatterer scatterer = plotParameters.ScatteringType switch
            {
                ScatteringType.PowerLaw => plotParameters.PowerLawScatterer,
                ScatteringType.Intralipid => plotParameters.IntralipidScatterer,
                ScatteringType.Mie => plotParameters.MieScatterer,
                _ => new PowerLawScatterer()
            };

            // get the wavelength
            plotParameters.Wavelengths = plotParameters.XAxis.AxisRange.AsEnumerable().ToArray();
            // set up the tissue
            plotParameters.Tissue = new Tissue(chromophoreAbsorbers, scatterer, plotParameters.TissueType);

            return _plotFactory.GetPlot(PlotType.Spectral, plotParameters);
        }
    }
}
