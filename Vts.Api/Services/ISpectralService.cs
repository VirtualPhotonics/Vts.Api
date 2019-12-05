using Vts.Api.Models;

namespace Vts.Api.Services
{
    public interface ISpectralService
    {
        string GetPlotData(SpectralPlotParameters plotParameters);
    }
}