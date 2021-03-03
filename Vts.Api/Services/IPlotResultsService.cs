using Vts.Api.Data;
using Vts.Api.Models;

namespace Vts.Api.Services
{
    public interface IPlotResultsService
    {
        Plots Plot(IPlotParameters plotParameters);

    }
}
