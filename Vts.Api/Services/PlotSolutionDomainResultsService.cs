using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Vts.Common;
using Vts.Extensions;
using Vts.Api.Data;
using Vts.Api.Models;
using Vts.Factories;

namespace Vts.Api.Services
{
    public class PlotSolutionDomainResultsService : IPlotResultsService
    {
        private readonly ILogger<PlotSolutionDomainResultsService> _logger;

        public PlotSolutionDomainResultsService(ILogger<PlotSolutionDomainResultsService> logger)
        {
            _logger = logger;
        }

        public string Plot(IPlotParameters plotParameters)
        {
            var msg = "";
            var parameters = (SolutionDomainPlotParameters) plotParameters;
            var fs = parameters.ForwardSolverType;
            var op = parameters.OpticalProperties;
            var xAxis = parameters.XAxis;
            var noise = parameters.NoiseValue;
            var independentAxis = parameters.IndependentAxes.Label;
            var independentValue = parameters.IndependentAxes.Value;
            var independentValues = xAxis.AsEnumerable().ToArray();
            try
            {
                IEnumerable<double> doubleResults;
                IEnumerable<Complex> complexResults;
                double[] xs;
                IEnumerable<Point> xyPoints, xyPointsReal, xyPointsImaginary;
                PlotData plotData, plotDataReal, plotDataImaginary;
                Plots plot;
                switch (parameters.SolutionDomain)
                {
                    case SolutionDomainType.ROfRho:
                        doubleResults = ROfRho(fs, op, xAxis, noise);
                        xs = independentValues;
                        xyPoints = xs.Zip(doubleResults, (x, y) => new Point(x, y));
                        plotData = new PlotData {Data = xyPoints, Label = "ROfRho"};
                        plot = new Plots
                        {
                            Id = "ROfRho", Detector = "R(ρ)", Legend = "R(ρ)", XAxis = "ρ", YAxis = "Reflectance",
                            PlotList = new List<PlotDataJson>()
                        };
                        plot.PlotList.Add(new PlotDataJson
                        {
                            Data = plotData.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                            Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp
                        });
                        msg = JsonConvert.SerializeObject(plot);
                        break;
                    case SolutionDomainType.ROfRhoAndTime:
                        if (independentAxis == "t")
                        {
                            var time = independentValue;
                            doubleResults = ROfRhoAndTime(fs, op, xAxis, time, noise);
                            xs = independentValues;
                            xyPoints = xs.Zip(doubleResults, (x, y) => new Point(x, y));
                            plotData = new PlotData {Data = xyPoints, Label = "ROfRhoAndTime"};
                            plot = new Plots
                            {
                                Id = "ROfRhoAndTimeFixedTime", Detector = "R(ρ,time)", Legend = "R(ρ,time)",
                                XAxis = "ρ", YAxis = "Reflectance", PlotList = new List<PlotDataJson>()
                            };
                            plot.PlotList.Add(new PlotDataJson
                            {
                                Data = plotData.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                                Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " t=" + time
                            });
                            msg = JsonConvert.SerializeObject(plot);
                        }
                        else
                        {
                            var rho = independentValue;
                            doubleResults = ROfRhoAndTime(fs, op, rho, xAxis, noise);
                            xs = independentValues.ToArray(); // the Skip(1) is giving inverse problems
                            xyPoints = xs.Zip(doubleResults, (x, y) => new Point(x, y));
                            plotData = new PlotData {Data = xyPoints, Label = "ROfRhoAndTime"};
                            plot = new Plots
                            {
                                Id = "ROfRhoAndTimeFixedRho", Detector = "R(ρ,time)", Legend = "R(ρ,time)",
                                XAxis = "Time", YAxis = "Reflectance", PlotList = new List<PlotDataJson>()
                            };
                            plot.PlotList.Add(new PlotDataJson
                            {
                                Data = plotData.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                                Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " ρ=" + rho
                            });
                            msg = JsonConvert.SerializeObject(plot);
                        }
                        break;
                    case SolutionDomainType.ROfRhoAndFt:
                        if (independentAxis == "ft")
                        {
                            var rho = xAxis;
                            var ft = independentValue;
                            complexResults = ROfRhoAndFt(fs, op, rho, ft, noise).ToArray();
                            xs = independentValues;
                            xyPointsReal = xs.Zip(complexResults, (x, y) => new Point(x, y.Real));
                            xyPointsImaginary = xs.Zip(complexResults, (x, y) => new Point(x, y.Imaginary));
                            plotDataReal = new PlotData {Data = xyPointsReal, Label = "ROfRhoAndFt"};
                            plotDataImaginary = new PlotData {Data = xyPointsImaginary, Label = "ROfRhoAndFt"};
                            plot = new Plots
                            {
                                Id = "ROfRhoAndFtFixedFt", Detector = "R(ρ,ft)", Legend = "R(ρ,ft)", XAxis = "ρ",
                                YAxis = "Reflectance", PlotList = new List<PlotDataJson>()
                            };
                            plot.PlotList.Add(new PlotDataJson
                            {
                                Data = plotDataReal.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                                Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " ft=" + ft + "(real)"
                            });
                            plot.PlotList.Add(new PlotDataJson
                            {
                                Data = plotDataImaginary.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                                Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " ft=" + ft + "(imag)"
                            });
                            msg = JsonConvert.SerializeObject(plot);
                        }
                        else
                        {
                            var rho = independentValue;
                            complexResults = ROfRhoAndFt(fs, op, rho, xAxis, noise).ToArray();
                            xs = independentValues;
                            xyPointsReal = xs.Zip(complexResults, (x, y) => new Point(x, y.Real));
                            xyPointsImaginary = xs.Zip(complexResults, (x, y) => new Point(x, y.Imaginary));
                            var realPlot = new PlotData {Data = xyPointsReal, Label = "ROfRhoAndFt"};
                            var imagPlot = new PlotData {Data = xyPointsImaginary, Label = "ROfRhoAndFt"};
                            var rhoPlot = new Plots
                            {
                                Id = "ROfRhoAndFtFixedRho", Detector = "R(ρ,ft)", Legend = "R(ρ,ft)", XAxis = "ft",
                                YAxis = "Reflectance", PlotList = new List<PlotDataJson>()
                            };
                            rhoPlot.PlotList.Add(new PlotDataJson
                            {
                                Data = realPlot.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                                Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " ρ=" + rho + "(real)"
                            });
                            rhoPlot.PlotList.Add(new PlotDataJson
                            {
                                Data = imagPlot.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                                Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " ρ=" + rho + "(imag)"
                            });
                            msg = JsonConvert.SerializeObject(rhoPlot);
                        }
                        break;
                    case SolutionDomainType.ROfFx:
                        doubleResults = ROfFx(fs, op, xAxis, noise);
                        xs = independentValues;
                        xyPoints = xs.Zip(doubleResults, (x, y) => new Point(x, y));
                        plotData = new PlotData {Data = xyPoints, Label = "ROfFx"};
                        plot = new Plots
                        {
                            Id = "ROfFx", Detector = "R(fx)", Legend = "R(fx)", XAxis = "fx", YAxis = "Reflectance",
                            PlotList = new List<PlotDataJson>()
                        };
                        plot.PlotList.Add(new PlotDataJson
                        {
                            Data = plotData.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                            Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp
                        });
                        msg = JsonConvert.SerializeObject(plot);
                        break;
                    case SolutionDomainType.ROfFxAndTime:
                        if (independentAxis == "t")
                        {
                            var time = independentValue;
                            doubleResults = ROfFxAndTime(fs, op, xAxis, time, noise);
                            xs = independentValues;
                            xyPoints = xs.Zip(doubleResults, (x, y) => new Point(x, y));
                            plotData = new PlotData {Data = xyPoints, Label = "ROfFxAndTime"};
                            plot = new Plots
                            {
                                Id = "ROfFxAndTimeFixedTime", Detector = "R(fx,time)", Legend = "R(fx,time)",
                                XAxis = "fx", YAxis = "Reflectance", PlotList = new List<PlotDataJson>()
                            };
                            plot.PlotList.Add(new PlotDataJson
                            {
                                Data = plotData.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                                Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " t=" + time
                            });
                            msg = JsonConvert.SerializeObject(plot);
                        }
                        else
                        {
                            var fx = independentValue;
                            doubleResults = ROfFxAndTime(fs, op, fx, xAxis, noise);
                            xs = independentValues;
                            xyPoints = xs.Zip(doubleResults, (x, y) => new Point(x, y));
                            plotData = new PlotData {Data = xyPoints, Label = "ROfFxAndTime"};
                            plot = new Plots
                            {
                                Id = "ROfFxAndTimeFixedFx", Detector = "R(fx,time)", Legend = "R(fx,time)",
                                XAxis = "Time", YAxis = "Reflectance", PlotList = new List<PlotDataJson>()
                            };
                            plot.PlotList.Add(new PlotDataJson
                            {
                                Data = plotData.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                                Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " ρ=" + fx
                            });
                            msg = JsonConvert.SerializeObject(plot);
                        }
                        break;
                    case SolutionDomainType.ROfFxAndFt:
                        if (independentAxis == "ft")
                        {
                            var ft = independentValue;
                            complexResults = ROfFxAndFt(fs, op, xAxis, ft, noise).ToArray();
                            xs = independentValues;
                            xyPointsReal = xs.Zip(complexResults, (x, y) => new Point(x, y.Real));
                            xyPointsImaginary = xs.Zip(complexResults, (x, y) => new Point(x, y.Imaginary));
                            plotDataReal = new PlotData {Data = xyPointsReal, Label = "ROfFxAndFt"};
                            plotDataImaginary = new PlotData {Data = xyPointsImaginary, Label = "ROfFxAndFt"};
                            plot = new Plots
                            {
                                Id = "ROfFxAndFtFixedFt", Detector = "R(fx,ft)", Legend = "R(fx,ft)", XAxis = "fx",
                                YAxis = "Reflectance", PlotList = new List<PlotDataJson>()
                            };
                            plot.PlotList.Add(new PlotDataJson
                            {
                                Data = plotDataReal.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                                Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " ft=" + ft + "(real)"
                            });
                            plot.PlotList.Add(new PlotDataJson
                            {
                                Data = plotDataImaginary.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                                Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " ft=" + ft + "(imag)"
                            });
                            msg = JsonConvert.SerializeObject(plot);
                        }
                        else
                        {
                            var fx = independentValue;
                            complexResults = ROfFxAndFt(fs, op, fx, xAxis, noise).ToArray();
                            xs = independentValues;
                            xyPointsReal = xs.Zip(complexResults, (x, y) => new Point(x, y.Real));
                            xyPointsImaginary = xs.Zip(complexResults, (x, y) => new Point(x, y.Imaginary));
                            plotDataReal = new PlotData {Data = xyPointsReal, Label = "ROfFxAndFt"};
                            plotDataImaginary = new PlotData {Data = xyPointsImaginary, Label = "ROfFxAndFt"};
                            plot = new Plots
                            {
                                Id = "ROfFxAndFtFixedFx", Detector = "R(fx,ft)", Legend = "R(fx,ft)", XAxis = "ft",
                                YAxis = "Reflectance", PlotList = new List<PlotDataJson>()
                            };
                            plot.PlotList.Add(new PlotDataJson
                            {
                                Data = plotDataReal.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                                Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " fx=" + fx + "(real)"
                            });
                            plot.PlotList.Add(new PlotDataJson
                            {
                                Data = plotDataImaginary.Data.Select(item => new List<double> {item.X, item.Y}).ToList(),
                                Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " fx=" + fx + "(imag)"
                            });
                            msg = JsonConvert.SerializeObject(plot);
                        }
                        break;
                }
                return msg;
            }
            catch (Exception e)
            {
                _logger.LogError("An error occurred: {Message}", e.Message);
                throw;
            }
        }

        private IEnumerable<double> ROfRho(ForwardSolverType forwardSolverType, OpticalProperties opticalProperties, DoubleRange rho, double noise)
        {
            var ops = opticalProperties.AsEnumerable();
            var rhos = rho.AsEnumerable();
            var fs = SolverFactory.GetForwardSolver(forwardSolverType);
            if (noise > 0.0)
            {
                return fs.ROfRho(ops, rhos).AddNoise(noise);
            }
            return fs.ROfRho(ops, rhos);
        }

        private IEnumerable<double> ROfRhoAndTime(ForwardSolverType forwardSolverType, OpticalProperties opticalProperties, DoubleRange rho, double time, double noise)
        {
            var ops = opticalProperties.AsEnumerable();
            var rhos = rho.AsEnumerable();
            var times = time.AsEnumerable();
            var fs = SolverFactory.GetForwardSolver(forwardSolverType);
            if (noise > 0.0)
            {
                return fs.ROfRhoAndTime(ops, rhos, times).AddNoise(noise);
            }
            return fs.ROfRhoAndTime(ops, rhos, times);
        }

        private IEnumerable<double> ROfRhoAndTime(ForwardSolverType forwardSolverType, OpticalProperties opticalProperties, double rho, DoubleRange time, double noise)
        {
            var ops = opticalProperties.AsEnumerable();
            var rhos = rho.AsEnumerable();
            var times = time.AsEnumerable();
            var fs = SolverFactory.GetForwardSolver(forwardSolverType);
            if (noise > 0.0)
            {
                return fs.ROfRhoAndTime(ops, rhos, times).AddNoise(noise);
            }
            return fs.ROfRhoAndTime(ops, rhos, times);
        }

        private IEnumerable<Complex> ROfRhoAndFt(ForwardSolverType forwardSolverType, OpticalProperties opticalProperties, DoubleRange rho, double ft, double noise)
        {
            var ops = opticalProperties.AsEnumerable().ToArray();
            var rhos = rho.AsEnumerable().ToArray();
            var fts = ft.AsEnumerable().ToArray();
            var fs = SolverFactory.GetForwardSolver(forwardSolverType);
            var results = fs.ROfRhoAndFt(ops, rhos, fts).ToArray();
            if (noise > 0.0)
            {
                var realsWithNoise = results.Select(r => r.Real).AddNoise(noise);
                var imagsWithNoise = results.Select(i => i.Imaginary).AddNoise(noise);
                IEnumerable<Complex> resultsWithNoise = realsWithNoise.Zip(imagsWithNoise, (a, b) => new Complex(a,b));
                return resultsWithNoise;
            }
            return fs.ROfRhoAndFt(ops, rhos, fts);
        }

        private IEnumerable<Complex> ROfRhoAndFt(ForwardSolverType forwardSolverType, OpticalProperties opticalProperties, double rho, DoubleRange ft, double noise)
        {
            var ops = opticalProperties.AsEnumerable().ToArray();
            var rhos = rho.AsEnumerable().ToArray();
            var fts = ft.AsEnumerable().ToArray();
            var fs = SolverFactory.GetForwardSolver(forwardSolverType);
            var results = fs.ROfRhoAndFt(ops, rhos, fts).ToArray();
            if (noise > 0.0)
            {
                var realsWithNoise = results.Select(r => r.Real).AddNoise(noise);
                var imagsWithNoise = results.Select(i => i.Imaginary).AddNoise(noise);
                IEnumerable<Complex> resultsWithNoise = realsWithNoise.Zip(imagsWithNoise, (a, b) => new Complex(a, b));
                return resultsWithNoise;
            }
            return fs.ROfRhoAndFt(ops, rhos, fts);
        }

        private IEnumerable<double> ROfFx(ForwardSolverType forwardSolverType, OpticalProperties opticalProperties, DoubleRange fx, double noise)
        {
            try
            {
                var ops = opticalProperties.AsEnumerable();
                var fxs = fx.AsEnumerable();
                var fs = SolverFactory.GetForwardSolver(forwardSolverType);
                if (noise > 0.0)
                {
                    return fs.ROfFx(ops, fxs).AddNoise(noise);
                }
                return fs.ROfFx(ops, fxs);
            }
            catch (Exception e)
            {
                _logger.LogError("Error in call to ROfFx: " + e.Message + "values fst: " + forwardSolverType + ", op: " + opticalProperties + ", rho:" + fx + " source: " + e.Source + " inner: " + e.InnerException);
                throw;
            }
        }

        private IEnumerable<double> ROfFxAndTime(ForwardSolverType forwardSolverType, OpticalProperties opticalProperties, DoubleRange fx, double time, double noise)
        {
            var ops = opticalProperties.AsEnumerable();
            var fxs = fx.AsEnumerable();
            var times = time.AsEnumerable();
            var fs = SolverFactory.GetForwardSolver(forwardSolverType);
            if (noise > 0.0)
            {
                return fs.ROfFxAndTime(ops, fxs, times).AddNoise(noise);
            }
            return fs.ROfFxAndTime(ops, fxs, times);
        }

        private IEnumerable<double> ROfFxAndTime(ForwardSolverType forwardSolverType, OpticalProperties opticalProperties, double fx, DoubleRange time, double noise)
        {
            var ops = opticalProperties.AsEnumerable();
            var fxs = fx.AsEnumerable();
            var times = time.AsEnumerable();
            var fs = SolverFactory.GetForwardSolver(forwardSolverType);
            if (noise > 0.0)
            {
                return fs.ROfFxAndTime(ops, fxs, times).AddNoise(noise);
            }
            return fs.ROfFxAndTime(ops, fxs, times);
        }

        private IEnumerable<Complex> ROfFxAndFt(ForwardSolverType forwardSolverType, OpticalProperties opticalProperties, DoubleRange fx, double ft, double noise)
        {
            var ops = opticalProperties.AsEnumerable().ToArray();
            var fxs = fx.AsEnumerable().ToArray();
            var fts = ft.AsEnumerable().ToArray();
            var fs = SolverFactory.GetForwardSolver(forwardSolverType);
            var results = fs.ROfRhoAndFt(ops, fxs, fts).ToArray();
            if (noise > 0.0)
            {
                var realsWithNoise = results.Select(r => r.Real).AddNoise(noise);
                var imagsWithNoise = results.Select(i => i.Imaginary).AddNoise(noise);
                IEnumerable<Complex> resultsWithNoise = realsWithNoise.Zip(imagsWithNoise, (a, b) => new Complex(a, b));
                return resultsWithNoise;
            }
            return fs.ROfFxAndFt(ops, fxs, fts);
        }

        private IEnumerable<Complex> ROfFxAndFt(ForwardSolverType forwardSolverType, OpticalProperties opticalProperties, double fx, DoubleRange ft, double noise)
        {
            var ops = opticalProperties.AsEnumerable().ToArray();
            var fxs = fx.AsEnumerable().ToArray();
            var fts = ft.AsEnumerable().ToArray();
            var fs = SolverFactory.GetForwardSolver(forwardSolverType);
            var results = fs.ROfRhoAndFt(ops, fxs, fts).ToArray();
            if (noise > 0.0)
            {
                var realsWithNoise = results.Select(r => r.Real).AddNoise(noise);
                var imagsWithNoise = results.Select(i => i.Imaginary).AddNoise(noise);
                IEnumerable<Complex> resultsWithNoise = realsWithNoise.Zip(imagsWithNoise, (a, b) => new Complex(a, b));
                return resultsWithNoise;
            }
            return fs.ROfFxAndFt(ops, fxs, fts);
        }
    }
}
