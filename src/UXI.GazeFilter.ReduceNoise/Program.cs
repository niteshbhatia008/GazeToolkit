using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using UXI.GazeToolkit;
using UXI.GazeToolkit.Fixations.VelocityThreshold;
using UXI.GazeToolkit.Smoothing;

namespace UXI.GazeFilter.ReduceNoise
{
    [Verb("movavg")]
    public class MovingAverageSmoothingOptions : BaseOptions, IMovingAverageSmoothingOptions
    {
        public NoiseReductionStrategy Strategy => NoiseReductionStrategy.MovingAverage;

        [Option('w', "window-size", Default = MovingAverageSmoothingFilter.DEFAULT_WINDOW_SIZE, HelpText = "Window size for Moving Average smoothing filter", Required = false)]
        public int WindowSize { get; set; }
    }


    [Verb("exponential")]
    public class ExponentialSmoothingOptions : BaseOptions, IExponentialSmoothingOptions
    {
        public NoiseReductionStrategy Strategy => NoiseReductionStrategy.Exponential;

        [Option('a', "alpha", Default = ExponentialSmoothingFilter.DEFAULT_ALPHA, HelpText = "Alpha parameter for Exponential fmoothing filter", Required = false)]
        public double Alpha { get; set; }
    }


    static class Program
    {
        static void Main(string[] args)
        {
            new FilterTool<SingleEyeGazeData, SingleEyeGazeData>
            (
                true,
                new Filter<SingleEyeGazeData, SingleEyeGazeData, MovingAverageSmoothingOptions>((s, o) => s.ReduceNoise(o)),
                new Filter<SingleEyeGazeData, SingleEyeGazeData, ExponentialSmoothingOptions>((s, o) => s.ReduceNoise(o))
            ).Execute(args);
        }
    }
}
