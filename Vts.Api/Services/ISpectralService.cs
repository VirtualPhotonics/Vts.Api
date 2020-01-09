using Vts.Api.Data;
using Vts.Api.Models;

namespace Vts.Api.Services
{
    public interface ISpectralService
    {
        Plots GetPlotData(SpectralPlotParameters plotParameters);
    }
}