﻿using Vts.Api.Data;
using Vts.Api.Enums;
using Vts.Api.Models;

namespace Vts.Api.Services
{
    public class PlotSpectralResultsService : IPlotResultsService
    {
        public PlotSpectralResultsService(ILogger<PlotSpectralResultsService> logger)
        {
        }

        public Plots Plot(IPlotParameters plotParameters)
        {
            var parameters = (SpectralPlotParameters)plotParameters;
            var xyPoints = new List<Point>();

            foreach (var wv in parameters.Wavelengths)
            {
                switch (parameters.SpectralPlotType)
                {
                    case SpectralPlotType.Mua:
                        xyPoints.Add(new Point(wv, parameters.Tissue.GetMua(wv)));
                        break;
                    case SpectralPlotType.Musp:
                        xyPoints.Add(new Point(wv, parameters.Tissue.GetMusp(wv)));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(plotParameters));
                }
            }
            var plotData = new PlotData { Data = xyPoints, Label = parameters.TissueType };
            var plot = new Plots
            {
                Id = "Spectral" + parameters.SpectralPlotType,
                PlotList = new List<PlotDataJson>()
            };
            plot.PlotList.Add(new PlotDataJson
            {
                Data = plotData.Data.Select(item => new List<double> { item.X, item.Y }).ToList(),
                Label = parameters.TissueType + " " + parameters.PlotName
            });
            return plot;
        }
    }
}
