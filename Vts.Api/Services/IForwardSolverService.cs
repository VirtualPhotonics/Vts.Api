using Vts.Api.Data;
using Vts.Api.Models;

namespace Vts.Api.Services
{
    public interface IForwardSolverService
    {
        Plots GetPlotData(SolutionDomainPlotParameters plotParameters);
    }
}
