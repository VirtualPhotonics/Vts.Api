using Vts.Api.Models;

namespace Vts.Api.Services
{
    public interface IPlotResultsService
    {
        string Plot(IPlotParameters plotParameters);
        
    }
}
