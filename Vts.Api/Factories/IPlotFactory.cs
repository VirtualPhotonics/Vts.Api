using Vts.Api.Enums;
using Vts.Api.Models;

namespace Vts.Api.Factories

{
    public interface IPlotFactory
    {
        string GetPlot(PlotType plotType, IPlotParameters plotParameters);
    }
}
