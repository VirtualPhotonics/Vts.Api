using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
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

        public string GetPlotData(SpectralPlotParameters plotParameters)
        {
            _logger.LogInformation("Get the plot data for the Spectral Panel");

            plotParameters.YAxis = plotParameters.PlotName;

            // set up the absorber concentrations
            var chromophoreAbsorbers = new List<IChromophoreAbsorber>();
            foreach (var absorber in plotParameters.AbsorberConcentration)
            {
                chromophoreAbsorbers.Add(new ChromophoreAbsorber(Enum.Parse<ChromophoreType>(absorber.Label, true), absorber.Value));
            }

            // set up the scatterer
            IScatterer scatterer;
            switch (plotParameters.ScatteringType)
            {
                case ScatteringType.PowerLaw:
                    scatterer = plotParameters.PowerLawScatterer;
                    break;
                case ScatteringType.Intralipid:
                    scatterer = plotParameters.IntralipidScatterer;
                    break;
                case ScatteringType.Mie:
                    scatterer = plotParameters.MieScatterer;
                    break;
                default:
                scatterer = new PowerLawScatterer();
                break;
            }

            // get the wavelength
            plotParameters.Wavelengths = plotParameters.XAxis.AsEnumerable().ToArray();
            // set up the tissue
            plotParameters.Tissue = new Tissue(chromophoreAbsorbers, scatterer, plotParameters.TissueType);

            return _plotFactory.GetPlot(PlotType.Spectral, plotParameters);
        }
    }
}
