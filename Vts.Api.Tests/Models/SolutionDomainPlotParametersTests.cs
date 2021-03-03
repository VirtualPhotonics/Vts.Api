using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using Vts.Api.Models;
using Vts.Common;

namespace Vts.Api.Tests.Models
{
    class SolutionDomainPlotParametersTests
    {
        [Test]
        public void Test_deserialize_forward_solver_data()
        {
            var postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRho\",\"xAxis\":{\"axis\":\"rho\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":19}},\"opticalProperties\":{\"title\":\"Optical Properties\",\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var xAxis = new DoubleRange(0.5, 9.5, 19);
            var opticalProperties = new OpticalProperties(0.01, 1, 0.8, 1.4);
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            Assert.AreEqual(xAxis.Start, solutionDomainPlotParameters.XAxis.AxisRange.Start);
            Assert.AreEqual(xAxis.Stop, solutionDomainPlotParameters.XAxis.AxisRange.Stop);
            Assert.AreEqual(xAxis.Count, solutionDomainPlotParameters.XAxis.AxisRange.Count);
            Assert.AreEqual(xAxis.Delta, solutionDomainPlotParameters.XAxis.AxisRange.Delta);
            Assert.AreEqual(ForwardSolverType.DistributedPointSourceSDA, solutionDomainPlotParameters.ForwardSolverType);
            Assert.AreEqual(ForwardSolverType.PointSourceSDA, solutionDomainPlotParameters.InverseSolverType);
            Assert.AreEqual(SolutionDomainType.ROfRho, solutionDomainPlotParameters.SolutionDomain);
            Assert.AreEqual(opticalProperties.Mua, solutionDomainPlotParameters.OpticalProperties.Mua);
            Assert.AreEqual(OptimizerType.MPFitLevenbergMarquardt, solutionDomainPlotParameters.OptimizerType);
            Assert.AreEqual(InverseFitType.MuaMusp, solutionDomainPlotParameters.OptimizationParameters);
            Assert.AreEqual(0, solutionDomainPlotParameters.NoiseValue);
            Assert.AreEqual(ForwardAnalysisType.R, solutionDomainPlotParameters.ModelAnalysis);
            Assert.IsNull(solutionDomainPlotParameters.MeasuredData);
        }

        [Test]
        public void Test_deserialize_forward_solver_with_wavelength()
        {
            var postData =
                "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRho\",\"xAxis\":{\"axis\":\"rho\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":19}},\"independentAxis\":{\"axis\":\"wavelength\",\"axisRange\":{\"start\":650,\"stop\":1000,\"count\":3}},\"wavelengthOpticalPropertyList\":[{\"mua\":0.0679,\"musp\":1.06,\"g\":0.8,\"n\":1.4},{\"mua\":0.04,\"musp\":0.934,\"g\":0.8,\"n\":1.4},{\"mua\":0.0678,\"musp\":0.84,\"g\":0.8,\"n\":1.4}],\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var xAxis = new DoubleRange(0.5, 9.5, 19);
            var independentAxis = new DoubleRange(650, 1000, 3);
            var opticalPropertyList = new[]
            {
                new OpticalProperties(0.0679, 1.06, 0.8, 1.4),
                new OpticalProperties(0.04, 0.934, 0.8, 1.4),
                new OpticalProperties(0.0678, 0.84, 0.8, 1.4)
            };
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            Assert.AreEqual(IndependentVariableAxis.Wavelength, solutionDomainPlotParameters.IndependentAxis.Axis);
            Assert.AreEqual(independentAxis.Start, solutionDomainPlotParameters.IndependentAxis.AxisRange.Start);
            Assert.AreEqual(independentAxis.Stop, solutionDomainPlotParameters.IndependentAxis.AxisRange.Stop);
            Assert.AreEqual(independentAxis.Count, solutionDomainPlotParameters.IndependentAxis.AxisRange.Count);
            Assert.AreEqual(independentAxis.Delta, solutionDomainPlotParameters.IndependentAxis.AxisRange.Delta);
            Assert.AreEqual(xAxis.Start, solutionDomainPlotParameters.XAxis.AxisRange.Start);
            Assert.AreEqual(xAxis.Stop, solutionDomainPlotParameters.XAxis.AxisRange.Stop);
            Assert.AreEqual(xAxis.Count, solutionDomainPlotParameters.XAxis.AxisRange.Count);
            Assert.AreEqual(xAxis.Delta, solutionDomainPlotParameters.XAxis.AxisRange.Delta);
            Assert.AreEqual(ForwardSolverType.DistributedPointSourceSDA, solutionDomainPlotParameters.ForwardSolverType);
            Assert.AreEqual(ForwardSolverType.PointSourceSDA, solutionDomainPlotParameters.InverseSolverType);
            Assert.AreEqual(SolutionDomainType.ROfRho, solutionDomainPlotParameters.SolutionDomain);
            Assert.AreEqual(opticalPropertyList[0].Mua, solutionDomainPlotParameters.WavelengthOpticalPropertyList[0].Mua);
            Assert.AreEqual(opticalPropertyList[1].Mua, solutionDomainPlotParameters.WavelengthOpticalPropertyList[1].Mua);
            Assert.AreEqual(opticalPropertyList[2].Musp, solutionDomainPlotParameters.WavelengthOpticalPropertyList[2].Musp);
            Assert.AreEqual(OptimizerType.MPFitLevenbergMarquardt, solutionDomainPlotParameters.OptimizerType);
            Assert.AreEqual(InverseFitType.MuaMusp, solutionDomainPlotParameters.OptimizationParameters);
            Assert.AreEqual(0, solutionDomainPlotParameters.NoiseValue);
            Assert.AreEqual(ForwardAnalysisType.R, solutionDomainPlotParameters.ModelAnalysis);
            Assert.IsNull(solutionDomainPlotParameters.MeasuredData);
        }


        [Test]
        public void Test_deserialize_forward_solver_rofrhoandt_with_wavelength()
        {
            var postData =
                "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfRho\",\"xAxis\":{\"axis\":\"rho\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":19}},\"secondIndependentAxis\":{\"axis\":\"time\",\"axisValue\":0.05},\"independentAxis\":{\"axis\":\"wavelength\",\"axisRange\":{\"start\":650,\"stop\":1000,\"count\":3}},\"wavelengthOpticalPropertyList\":[{\"mua\":0.0679,\"musp\":1.06,\"g\":0.8,\"n\":1.4},{\"mua\":0.04,\"musp\":0.934,\"g\":0.8,\"n\":1.4},{\"mua\":0.0678,\"musp\":0.84,\"g\":0.8,\"n\":1.4}],\"modelAnalysis\":\"R\",\"noiseValue\":\"0\"}";
            var xAxis = new DoubleRange(0.5, 9.5, 19);
            var independentAxis = new DoubleRange(650, 1000, 3);
            var opticalPropertyList = new[]
            {
                new OpticalProperties(0.0679, 1.06, 0.8, 1.4),
                new OpticalProperties(0.04, 0.934, 0.8, 1.4),
                new OpticalProperties(0.0678, 0.84, 0.8, 1.4)
            };
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            Assert.AreEqual(IndependentVariableAxis.Time, solutionDomainPlotParameters.SecondIndependentAxis.Axis); Assert.AreEqual(0.05, solutionDomainPlotParameters.SecondIndependentAxis.AxisValue);
            Assert.AreEqual(IndependentVariableAxis.Wavelength, solutionDomainPlotParameters.IndependentAxis.Axis);
            Assert.AreEqual(independentAxis.Start, solutionDomainPlotParameters.IndependentAxis.AxisRange.Start);
            Assert.AreEqual(independentAxis.Stop, solutionDomainPlotParameters.IndependentAxis.AxisRange.Stop);
            Assert.AreEqual(independentAxis.Count, solutionDomainPlotParameters.IndependentAxis.AxisRange.Count);
            Assert.AreEqual(independentAxis.Delta, solutionDomainPlotParameters.IndependentAxis.AxisRange.Delta);
            Assert.AreEqual(xAxis.Start, solutionDomainPlotParameters.XAxis.AxisRange.Start);
            Assert.AreEqual(xAxis.Stop, solutionDomainPlotParameters.XAxis.AxisRange.Stop);
            Assert.AreEqual(xAxis.Count, solutionDomainPlotParameters.XAxis.AxisRange.Count);
            Assert.AreEqual(xAxis.Delta, solutionDomainPlotParameters.XAxis.AxisRange.Delta);
            Assert.AreEqual(ForwardSolverType.DistributedPointSourceSDA, solutionDomainPlotParameters.ForwardSolverType);
            Assert.AreEqual(ForwardSolverType.PointSourceSDA, solutionDomainPlotParameters.InverseSolverType);
            Assert.AreEqual(SolutionDomainType.ROfRho, solutionDomainPlotParameters.SolutionDomain);
            Assert.AreEqual(opticalPropertyList[0].Mua, solutionDomainPlotParameters.WavelengthOpticalPropertyList[0].Mua);
            Assert.AreEqual(opticalPropertyList[1].Mua, solutionDomainPlotParameters.WavelengthOpticalPropertyList[1].Mua);
            Assert.AreEqual(opticalPropertyList[2].Musp, solutionDomainPlotParameters.WavelengthOpticalPropertyList[2].Musp);
            Assert.AreEqual(OptimizerType.MPFitLevenbergMarquardt, solutionDomainPlotParameters.OptimizerType);
            Assert.AreEqual(InverseFitType.MuaMusp, solutionDomainPlotParameters.OptimizationParameters);
            Assert.AreEqual(0, solutionDomainPlotParameters.NoiseValue);
            Assert.AreEqual(ForwardAnalysisType.R, solutionDomainPlotParameters.ModelAnalysis);
            Assert.IsNull(solutionDomainPlotParameters.MeasuredData);
        }

        [Test]
        public void Test_deserialize_forward_solver_roffxandft()
        {
            var postData = "{\"forwardSolverType\":\"DistributedPointSourceSDA\",\"solutionDomain\":\"ROfFxAndFt\",\"IndependentAxis\":{\"axis\":\"ft\",\"axisValue\":0.05},\"xAxis\":{\"axis\":\"fx\",\"axisRange\":{\"start\":0,\"stop\":0.5,\"count\":51}},\"opticalProperties\":{\"title\":\"Optical Properties\",\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4},\"modelAnalysis\":\"dRdMusp\",\"noiseValue\":\"0\"}";
            var xAxis = new DoubleRange(0, 0.5, 51);
            var opticalProperties = new OpticalProperties(0.01, 1, 0.8, 1.4);
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            Assert.AreEqual(xAxis.Start, solutionDomainPlotParameters.XAxis.AxisRange.Start);
            Assert.AreEqual(xAxis.Stop, solutionDomainPlotParameters.XAxis.AxisRange.Stop);
            Assert.AreEqual(xAxis.Count, solutionDomainPlotParameters.XAxis.AxisRange.Count);
            Assert.AreEqual(xAxis.Delta, solutionDomainPlotParameters.XAxis.AxisRange.Delta);
            Assert.AreEqual(ForwardSolverType.DistributedPointSourceSDA, solutionDomainPlotParameters.ForwardSolverType);
            Assert.AreEqual(ForwardSolverType.PointSourceSDA, solutionDomainPlotParameters.InverseSolverType);
            Assert.AreEqual(SolutionDomainType.ROfFxAndFt, solutionDomainPlotParameters.SolutionDomain);
            Assert.AreEqual(opticalProperties.Mua, solutionDomainPlotParameters.OpticalProperties.Mua);
            Assert.AreEqual(OptimizerType.MPFitLevenbergMarquardt, solutionDomainPlotParameters.OptimizerType);
            Assert.AreEqual(InverseFitType.MuaMusp, solutionDomainPlotParameters.OptimizationParameters);
            Assert.AreEqual(0, solutionDomainPlotParameters.NoiseValue);
            Assert.AreEqual(ForwardAnalysisType.dRdMusp, solutionDomainPlotParameters.ModelAnalysis);
            Assert.IsNull(solutionDomainPlotParameters.MeasuredData);
        }

        [Test]
        public void Test_deserialize_inverse_solver_data()
        {
            var postData = "{\"inverseSolverType\":\"PointSourceSDA\",\"optimizerType\":\"MPFitLevenbergMarquardt\",\"optimizationParameters\":\"MuaMusp\",\"solutionDomain\":\"ROfRho\",\"measuredData\":[[0.5,0.034828428710495074],[0.7571428571428571,0.029420531983087726],[1.0142857142857142,0.02093360911749075],[1.2714285714285714,0.01665715182769329],[1.5285714285714285,0.014364183918250022],[1.7857142857142856,0.011454828142680954],[2.0428571428571427,0.009079166450343084],[2.3,0.00766922059364649],[2.557142857142857,0.005969062618866233],[2.814285714285714,0.005133772121534473],[3.071428571428571,0.004173573203610217],[3.3285714285714283,0.0037753248321977808],[3.5857142857142854,0.0033053865379579577],[3.8428571428571425,0.0025773098635545667],[4.1,0.00232888357135617],[4.357142857142857,0.001995707298051381],[4.614285714285714,0.0016130076959352349],[4.871428571428571,0.0015272034490867183],[5.128571428571428,0.0013992385402248396],[5.385714285714285,0.0011945241210807522],[5.642857142857142,0.0010810369822821084],[5.8999999999999995,0.0009448803801525864],[6.157142857142857,0.0009198316127711655],[6.414285714285714,0.0006943417260433173],[6.671428571428571,0.0006333583166048397],[6.928571428571428,0.0006216988605675773],[7.185714285714285,0.000512835051348594],[7.442857142857142,0.0004826500754318199],[7.699999999999999,0.00043617857542623087],[7.957142857142856,0.0003878849790650086],[8.214285714285714,0.000378880207382442],[8.471428571428572,0.00031667904143385504],[8.728571428571428,0.00026887511680669254],[8.985714285714284,0.00025470774216120143],[9.24285714285714,0.00022926547222261112],[9.499999999999996,0.000201735054811538]],\"independentAxis\":{\"axis\":\"time\",\"axisValue\":0.05},\"xAxis\": {\"axis\": \"rho\",\"axisRange\":{\"start\":0.5,\"stop\":9.5,\"count\":\"36\"}},\"opticalProperties\":{\"mua\":0.01,\"musp\":1,\"g\":0.8,\"n\":1.4}}";
            var xAxis = new DoubleRange(0.5, 9.5, 36);
            var opticalProperties = new OpticalProperties(0.01, 1, 0.8, 1.4);
            var measuredData = new List<double[]> {
                 new [] { 0.5, 0.034828428710495074 },
                 new [] { 0.7571428571428571, 0.029420531983087726 },
                 new [] { 1.0142857142857142, 0.02093360911749075 },
                 new [] { 1.2714285714285714, 0.01665715182769329 },
                 new [] { 1.5285714285714285, 0.014364183918250022 },
                 new [] { 1.7857142857142856, 0.011454828142680954 },
                 new [] { 2.0428571428571427, 0.009079166450343084 },
                 new [] { 2.3, 0.00766922059364649 },
                 new [] { 2.557142857142857, 0.005969062618866233 },
                 new [] { 2.814285714285714, 0.005133772121534473 }
            };
            var solutionDomainPlotParameters = JsonConvert.DeserializeObject<SolutionDomainPlotParameters>(postData);
            Assert.AreEqual(xAxis.Start, solutionDomainPlotParameters.XAxis.AxisRange.Start);
            Assert.AreEqual(xAxis.Stop, solutionDomainPlotParameters.XAxis.AxisRange.Stop);
            Assert.AreEqual(xAxis.Count, solutionDomainPlotParameters.XAxis.AxisRange.Count);
            Assert.AreEqual(xAxis.Delta, solutionDomainPlotParameters.XAxis.AxisRange.Delta);
            Assert.AreEqual(ForwardSolverType.PointSourceSDA, solutionDomainPlotParameters.ForwardSolverType);
            Assert.AreEqual(ForwardSolverType.PointSourceSDA, solutionDomainPlotParameters.InverseSolverType);
            Assert.AreEqual(SolutionDomainType.ROfRho, solutionDomainPlotParameters.SolutionDomain);
            Assert.AreEqual(opticalProperties.Mua, solutionDomainPlotParameters.OpticalProperties.Mua);
            Assert.AreEqual(OptimizerType.MPFitLevenbergMarquardt, solutionDomainPlotParameters.OptimizerType);
            Assert.AreEqual(InverseFitType.MuaMusp, solutionDomainPlotParameters.OptimizationParameters);
            Assert.AreEqual(0, solutionDomainPlotParameters.NoiseValue);
            Assert.AreEqual(ForwardAnalysisType.R, solutionDomainPlotParameters.ModelAnalysis);
            Assert.AreEqual(measuredData[3][0], solutionDomainPlotParameters.MeasuredData[3][0]);
            Assert.AreEqual(measuredData[7][1], solutionDomainPlotParameters.MeasuredData[7][1]);
            Assert.AreEqual(measuredData[5][0], solutionDomainPlotParameters.MeasuredData[5][0]);
        }
    }
}
