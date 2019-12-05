using System;

namespace Vts.Api.Enums
{
    public enum PlotType
    {
        /// <summary>
        /// Plots generated from Forward and Inverse panels
        /// </summary>
        SolutionDomain,
        /// <summary>
        /// Plots generated from Spectral panel
        /// </summary>
        Spectral,
    }

    public enum SpectralPlotType
    {
        /// <summary>
        /// mua spectra
        /// </summary>
        Mua,
        /// <summary>
        /// mus' spectra
        /// </summary>
        Musp
    }
}
