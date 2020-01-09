using Vts.Api.Data;
using Vts.Api.Enums;
using Vts.Api.Models;

namespace Vts.Api.Factories

{
    public interface IPlotFactory
    {
        Plots GetPlot(PlotType plotType, IPlotParameters plotParameters);
    }
}
