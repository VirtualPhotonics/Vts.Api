using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json;
using Vts.Common;
using Vts.Extensions;
using Vts.Api.Data;

namespace Vts.Api.Services
{
    public class ForwardSolverService
    {
        public string GetPlotData(dynamic values)
        {
            try
            {
                dynamic vtsSettings = values;
                var sd = vtsSettings["solutionDomain"];
                var fs = vtsSettings["forwardSolverEngine"];
                var msg = "";
                var op = new OpticalProperties((double)vtsSettings["opticalProperties"]["mua"], (double)vtsSettings["opticalProperties"]["musp"], (double)vtsSettings["opticalProperties"]["g"], (double)vtsSettings["opticalProperties"]["n"]);
                if (sd == "rofrho")
                {
                    var rho = new DoubleRange((double)vtsSettings["range"]["startValue"], (double)vtsSettings["range"]["endValue"], (int)vtsSettings["range"]["numberValue"]);
                    IEnumerable<double> results = null;
                    results = ROfRho(Enum.Parse(typeof(ForwardSolverType), fs.ToString()), op, rho);
                    var independentValues = rho.AsEnumerable().ToArray();
                    var rhos = independentValues.Skip(1).Zip(independentValues.Take(independentValues.Length - 1), (first, second) => (first + second) / 2).ToArray();

                    var rOfRhoPoints = rhos.Zip(results, (x, y) => new Point(x, y));
                    var rOfRhoPlot = new PlotData { Data = rOfRhoPoints, Label = "ROfRho" };
                    var rhoPlot = new Plots { Id = "ROfRho", Detector = "R(ρ)", Legend = "R(ρ)", XAxis = "ρ", YAxis = "Reflectance", PlotList = new List<PlotDataJson>() };
                    rhoPlot.PlotList.Add(new PlotDataJson { Data = rOfRhoPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp });
                    msg = JsonConvert.SerializeObject(rhoPlot);
                }
                else if (sd == "rofrhoandt")
                {
                    IEnumerable<double> results = null;
                    var independentAxis = vtsSettings["independentAxes"]["label"];
                    if (independentAxis == "t")
                    {
                        var rho = new DoubleRange((double)vtsSettings["range"]["startValue"], (double)vtsSettings["range"]["endValue"], (int)vtsSettings["range"]["numberValue"]);
                        var time = (double)vtsSettings["independentAxes"]["value"];
                        results = ROfRhoAndTime(Enum.Parse(typeof(ForwardSolverType), fs.ToString()), op, rho, time);
                        var independentValues = rho.AsEnumerable().ToArray();
                        var rhos = independentValues.Skip(1).Zip(independentValues.Take(independentValues.Length - 1), (first, second) => (first + second) / 2).ToArray();
                        var rOfRhoPoints = rhos.Zip(results, (x, y) => new Point(x, y));
                        var rOfRhoPlot = new PlotData { Data = rOfRhoPoints, Label = "ROfRhoAndTime" };
                        var rhoPlot = new Plots { Id = "ROfRhoAndTimeFixedTime", Detector = "R(ρ,time)", Legend = "R(ρ,time)", XAxis = "ρ", YAxis = "Reflectance", PlotList = new List<PlotDataJson>() };
                        rhoPlot.PlotList.Add(new PlotDataJson { Data = rOfRhoPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " t=" + time });
                        msg = JsonConvert.SerializeObject(rhoPlot);
                    }
                    else
                    {
                        var time = new DoubleRange((double)vtsSettings["range"]["startValue"], (double)vtsSettings["range"]["endValue"], (int)vtsSettings["range"]["numberValue"]);
                        var rho = (double)vtsSettings["independentAxes"]["value"];
                        results = ROfRhoAndTime(Enum.Parse(typeof(ForwardSolverType), fs.ToString()), op, rho, time);
                        var independentValues = time.AsEnumerable().ToArray();
                        var times = independentValues.Skip(1).Zip(independentValues.Take(independentValues.Length - 1), (first, second) => (first + second) / 2).ToArray();
                        var dataPoints = times.Zip(results, (x, y) => new Point(x, y));
                        var dataPlot = new PlotData { Data = dataPoints, Label = "ROfRhoAndTime" };
                        var rhoPlot = new Plots { Id = "ROfRhoAndTimeFixedRho", Detector = "R(ρ,time)", Legend = "R(ρ,time)", XAxis = "Time", YAxis = "Reflectance", PlotList = new List<PlotDataJson>() };
                        rhoPlot.PlotList.Add(new PlotDataJson { Data = dataPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " ρ=" + rho });
                        msg = JsonConvert.SerializeObject(rhoPlot);
                    }
                }
                else if (sd == "rofrhoandft")
                {
                    IEnumerable<Complex> results;
                    var independentAxis = vtsSettings["independentAxes"]["label"];
                    if (independentAxis == "ft")
                    {
                        var rho = new DoubleRange((double)vtsSettings["range"]["startValue"], (double)vtsSettings["range"]["endValue"], (int)vtsSettings["range"]["numberValue"]);
                        var ft = (double)vtsSettings["independentAxes"]["value"];
                        results = ROfRhoAndFt(Enum.Parse(typeof(ForwardSolverType), fs.ToString()), op, rho, ft);
                        var independentValues = rho.AsEnumerable().ToArray();
                        var rhos = independentValues.Skip(1).Zip(independentValues.Take(independentValues.Length - 1), (first, second) => (first + second) / 2).ToArray();
                        var realPoints = rhos.Zip(results, (x, y) => new Point(x, y.Real));
                        var imagPoints = rhos.Zip(results, (x, y) => new Point(x, y.Imaginary));
                        var realPlot = new PlotData { Data = realPoints, Label = "ROfRhoAndFt" };
                        var imagPlot = new PlotData { Data = imagPoints, Label = "ROfRhoAndFt" };
                        var rhoPlot = new Plots { Id = "ROfRhoAndFtFixedFt", Detector = "R(ρ,ft)", Legend = "R(ρ,ft)", XAxis = "ρ", YAxis = "Reflectance", PlotList = new List<PlotDataJson>() };
                        rhoPlot.PlotList.Add(new PlotDataJson { Data = realPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " t=" + ft + "(real)" });
                        rhoPlot.PlotList.Add(new PlotDataJson { Data = imagPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " t=" + ft + "(imag)" });
                        msg = JsonConvert.SerializeObject(rhoPlot);
                    }
                    else
                    {
                        var ft = new DoubleRange((double)vtsSettings["range"]["startValue"], (double)vtsSettings["range"]["endValue"], (int)vtsSettings["range"]["numberValue"]);
                        var rho = (double)vtsSettings["independentAxes"]["value"];
                        results = ROfRhoAndFt(Enum.Parse(typeof(ForwardSolverType), fs.ToString()), op, rho, ft);
                        var independentValues = ft.AsEnumerable().ToArray();
                        var times = independentValues.Skip(1).Zip(independentValues.Take(independentValues.Length - 1), (first, second) => (first + second) / 2).ToArray();
                        var realPoints = times.Zip(results, (x, y) => new Point(x, y.Real));
                        var imagPoints = times.Zip(results, (x, y) => new Point(x, y.Imaginary));
                        var realPlot = new PlotData { Data = realPoints, Label = "ROfRhoAndFt" };
                        var imagPlot = new PlotData { Data = imagPoints, Label = "ROfRhoAndFt" };
                        var rhoPlot = new Plots { Id = "ROfRhoAndFtFixedRho", Detector = "R(ρ,ft)", Legend = "R(ρ,ft)", XAxis = "Time", YAxis = "Reflectance", PlotList = new List<PlotDataJson>() };
                        rhoPlot.PlotList.Add(new PlotDataJson { Data = realPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " ρ=" + rho + "(real)" });
                        rhoPlot.PlotList.Add(new PlotDataJson { Data = imagPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " ρ=" + rho + "(imag)" });
                        msg = JsonConvert.SerializeObject(rhoPlot);
                    }
                }
                else if (sd == "roffx")
                {
                    var fx = new DoubleRange((double)vtsSettings["range"]["startValue"], (double)vtsSettings["range"]["endValue"], (int)vtsSettings["range"]["numberValue"]);
                    IEnumerable<double> results = null;
                    results = ROfFx(Enum.Parse(typeof(ForwardSolverType), fs.ToString()), op, fx);
                    var independentValues = fx.AsEnumerable().ToArray();
                    var fxs = independentValues.Skip(1).Zip(independentValues.Take(independentValues.Length - 1), (first, second) => (first + second) / 2).ToArray();

                    var rOfFxPoints = fxs.Zip(results, (x, y) => new Point(x, y));
                    var rOfFxPlot = new PlotData { Data = rOfFxPoints, Label = "ROfFx" };
                    var fxPlot = new Plots { Id = "ROfFx", Detector = "R(fx)", Legend = "R(fx)", XAxis = "fx", YAxis = "Reflectance", PlotList = new List<PlotDataJson>() };
                    fxPlot.PlotList.Add(new PlotDataJson { Data = rOfFxPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp });
                    msg = JsonConvert.SerializeObject(fxPlot);
                }
                else if (sd == "roffxandt")
                {
                    IEnumerable<double> results = null;
                    var independentAxis = vtsSettings["independentAxes"]["label"];
                    if (independentAxis == "t")
                    {
                        var fx = new DoubleRange((double)vtsSettings["range"]["startValue"], (double)vtsSettings["range"]["endValue"], (int)vtsSettings["range"]["numberValue"]);
                        var time = (double)vtsSettings["independentAxes"]["value"];
                        results = ROfFxAndTime(Enum.Parse(typeof(ForwardSolverType), fs.ToString()), op, fx, time);
                        var independentValues = fx.AsEnumerable().ToArray();
                        var fxs = independentValues.Skip(1).Zip(independentValues.Take(independentValues.Length - 1), (first, second) => (first + second) / 2).ToArray();
                        var rOfFxPoints = fxs.Zip(results, (x, y) => new Point(x, y));
                        var rOfFxPlot = new PlotData { Data = rOfFxPoints, Label = "ROfFxAndTime" };
                        var fxPlot = new Plots { Id = "ROfFxAndTimeFixedTime", Detector = "R(fx,time)", Legend = "R(fx,time)", XAxis = "fx", YAxis = "Reflectance", PlotList = new List<PlotDataJson>() };
                        fxPlot.PlotList.Add(new PlotDataJson { Data = rOfFxPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " t=" + time });
                        msg = JsonConvert.SerializeObject(fxPlot);
                    }
                    else
                    {
                        var time = new DoubleRange((double)vtsSettings["range"]["startValue"], (double)vtsSettings["range"]["endValue"], (int)vtsSettings["range"]["numberValue"]);
                        var fx = (double)vtsSettings["independentAxes"]["value"];
                        results = ROfFxAndTime(Enum.Parse(typeof(ForwardSolverType), fs.ToString()), op, fx, time);
                        var independentValues = time.AsEnumerable().ToArray();
                        var times = independentValues.Skip(1).Zip(independentValues.Take(independentValues.Length - 1), (first, second) => (first + second) / 2).ToArray();
                        var dataPoints = times.Zip(results, (x, y) => new Point(x, y));
                        var dataPlot = new PlotData { Data = dataPoints, Label = "ROfFxAndTime" };
                        var fxPlot = new Plots { Id = "ROfFxAndTimeFixedFx", Detector = "R(fx,time)", Legend = "R(fx,time)", XAxis = "Time", YAxis = "Reflectance", PlotList = new List<PlotDataJson>() };
                        fxPlot.PlotList.Add(new PlotDataJson { Data = dataPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " ρ=" + fx });
                        msg = JsonConvert.SerializeObject(fxPlot);
                    }
                }
                else if (sd == "roffxandft")
                {
                    IEnumerable<Complex> results;
                    var independentAxis = vtsSettings["independentAxes"]["label"];
                    if (independentAxis == "ft")
                    {
                        var fx = new DoubleRange((double)vtsSettings["range"]["startValue"], (double)vtsSettings["range"]["endValue"], (int)vtsSettings["range"]["numberValue"]);
                        var ft = (double)vtsSettings["independentAxes"]["value"];
                        results = ROfFxAndFt(Enum.Parse(typeof(ForwardSolverType), fs.ToString()), op, fx, ft);
                        var independentValues = fx.AsEnumerable().ToArray();
                        var fxs = independentValues.Skip(1).Zip(independentValues.Take(independentValues.Length - 1), (first, second) => (first + second) / 2).ToArray();
                        var realPoints = fxs.Zip(results, (x, y) => new Point(x, y.Real));
                        var imagPoints = fxs.Zip(results, (x, y) => new Point(x, y.Imaginary));
                        var realPlot = new PlotData { Data = realPoints, Label = "ROfFxAndFt" };
                        var imagPlot = new PlotData { Data = imagPoints, Label = "ROfFxAndFt" };
                        var fxPlot = new Plots { Id = "ROfFxAndFtFixedFt", Detector = "R(fx,ft)", Legend = "R(fx,ft)", XAxis = "fx", YAxis = "Reflectance", PlotList = new List<PlotDataJson>() };
                        fxPlot.PlotList.Add(new PlotDataJson { Data = realPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " t=" + ft + "(real)" });
                        fxPlot.PlotList.Add(new PlotDataJson { Data = imagPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs + " μa=" + op.Mua + " μs'=" + op.Musp + " t=" + ft + "(imag)" });
                        msg = JsonConvert.SerializeObject(fxPlot);
                    }
                    else
                    {
                        var ft = new DoubleRange((double)vtsSettings["range"]["startValue"], (double)vtsSettings["range"]["endValue"], (int)vtsSettings["range"]["numberValue"]);
                        var fx = (double)vtsSettings["independentAxes"]["value"];
                        results = ROfFxAndFt(Enum.Parse(typeof(ForwardSolverType), fs.ToString()), op, fx, ft);
                        var independentValues = ft.AsEnumerable().ToArray();
                        var times = independentValues.Skip(1).Zip(independentValues.Take(independentValues.Length - 1), (first, second) => (first + second) / 2).ToArray();
                        var realPoints = times.Zip(results, (x, y) => new Point(x, y.Real));
                        var imagPoints = times.Zip(results, (x, y) => new Point(x, y.Imaginary));
                        var realPlot = new PlotData { Data = realPoints, Label = "ROfFxAndFt" };
                        var imagPlot = new PlotData { Data = imagPoints, Label = "ROfFxAndFt" };
                        var fxPlot = new Plots { Id = "ROfFxAndFtFixedFx", Detector = "R(fx,ft)", Legend = "R(fx,ft)", XAxis = "Time", YAxis = "Reflectance", PlotList = new List<PlotDataJson>() };
                        fxPlot.PlotList.Add(new PlotDataJson { Data = realPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs.ToString() + " μa=" + op.Mua + " μs'=" + op.Musp + " fx=" + fx + "(real)" });
                        fxPlot.PlotList.Add(new PlotDataJson { Data = imagPlot.Data.Select(item => new List<double> { item.X, item.Y }).ToList(), Label = fs.ToString() + " μa=" + op.Mua + " μs'=" + op.Musp + " fx=" + fx + "(imag)" });
                        msg = JsonConvert.SerializeObject(fxPlot);
                    }
                }
                return msg;
            }
            catch (Exception e)
            {
                throw new Exception("Error during action: " + e.Message);
            }
        }

        private IEnumerable<double> ROfRho(ForwardSolverType fst, OpticalProperties op, DoubleRange rho)
        {
            var ops = op.AsEnumerable();
            var rhos = rho.AsEnumerable();
            var fs = Factories.SolverFactory.GetForwardSolver(fst);
            return fs.ROfRho(ops, rhos);
        }

        private IEnumerable<double> ROfRhoAndTime(ForwardSolverType fst, OpticalProperties op, DoubleRange rho, double time)
        {
            var ops = op.AsEnumerable();
            var rhos = rho.AsEnumerable();
            var times = time.AsEnumerable();
            var fs = Factories.SolverFactory.GetForwardSolver(fst);
            return fs.ROfRhoAndTime(ops, rhos, times);
        }

        private IEnumerable<double> ROfRhoAndTime(ForwardSolverType fst, OpticalProperties op, double rho, DoubleRange time)
        {
            var ops = op.AsEnumerable();
            var rhos = rho.AsEnumerable();
            var times = time.AsEnumerable();
            var fs = Factories.SolverFactory.GetForwardSolver(fst);
            return fs.ROfRhoAndTime(ops, rhos, times);
        }

        private IEnumerable<Complex> ROfRhoAndFt(ForwardSolverType fst, OpticalProperties op, DoubleRange rho, double ft)
        {
            var ops = op.AsEnumerable();
            var rhos = rho.AsEnumerable();
            var fts = ft.AsEnumerable();
            var fs = Factories.SolverFactory.GetForwardSolver(fst);
            return fs.ROfRhoAndFt(ops, rhos, fts);
        }

        private IEnumerable<Complex> ROfRhoAndFt(ForwardSolverType fst, OpticalProperties op, double rho, DoubleRange ft)
        {
            var ops = op.AsEnumerable();
            var rhos = rho.AsEnumerable();
            var fts = ft.AsEnumerable();
            var fs = Factories.SolverFactory.GetForwardSolver(fst);
            return fs.ROfRhoAndFt(ops, rhos, fts);
        }

        private IEnumerable<double> ROfFx(ForwardSolverType fst, OpticalProperties op, DoubleRange fx)
        {
            try
            {
                var ops = op.AsEnumerable();
                var rhos = fx.AsEnumerable();
                var fs = Factories.SolverFactory.GetForwardSolver(fst);
                return fs.ROfFx(ops, rhos);
            }
            catch (Exception e)
            {
                throw new Exception("Error in call to ROfFx: " + e.Message + "values fst: " + fst + ", op: " + op + ", rho:" + fx + " source: " + e.Source + " inner: " + e.InnerException);
            }
        }

        private IEnumerable<double> ROfFxAndTime(ForwardSolverType fst, OpticalProperties op, DoubleRange fx, double time)
        {
            var ops = op.AsEnumerable();
            var fxs = fx.AsEnumerable();
            var times = time.AsEnumerable();
            var fs = Factories.SolverFactory.GetForwardSolver(fst);
            return fs.ROfFxAndTime(ops, fxs, times);
        }

        private IEnumerable<double> ROfFxAndTime(ForwardSolverType fst, OpticalProperties op, double fx, DoubleRange time)
        {
            var ops = op.AsEnumerable();
            var fxs = fx.AsEnumerable();
            var times = time.AsEnumerable();
            var fs = Factories.SolverFactory.GetForwardSolver(fst);
            return fs.ROfFxAndTime(ops, fxs, times);
        }

        private IEnumerable<Complex> ROfFxAndFt(ForwardSolverType fst, OpticalProperties op, DoubleRange fx, double ft)
        {
            var ops = op.AsEnumerable();
            var fxs = fx.AsEnumerable();
            var fts = ft.AsEnumerable();
            var fs = Factories.SolverFactory.GetForwardSolver(fst);
            return fs.ROfFxAndFt(ops, fxs, fts);
        }

        private IEnumerable<Complex> ROfFxAndFt(ForwardSolverType fst, OpticalProperties op, double fx, DoubleRange ft)
        {
            var ops = op.AsEnumerable();
            var fxs = fx.AsEnumerable();
            var fts = ft.AsEnumerable();
            var fs = Factories.SolverFactory.GetForwardSolver(fst);
            return fs.ROfFxAndFt(ops, fxs, fts);
        }
    }
}
