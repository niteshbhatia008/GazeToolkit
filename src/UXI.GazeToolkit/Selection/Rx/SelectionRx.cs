using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Extensions;

namespace UXI.GazeToolkit.Selection
{
    public enum EyeSelectionStrategy
    {
        Left,
        Right,
        Average,
        StrictAverage
    }



    public interface IEyeSelectionOptions
    {
        EyeSelectionStrategy Strategy { get; }
    }



    public static class SelectionRx
    {
        private static ISingleEyeGazeDataSelector ResolveSelector(EyeSelectionStrategy strategy)
        {
            switch (strategy)
            {
                case EyeSelectionStrategy.Left:
                    return LeftEyeSelector.Instance;
                case EyeSelectionStrategy.Right:
                    return RightEyeSelector.Instance;
                case EyeSelectionStrategy.Average:
                    return AverageSelector.Instance;
                case EyeSelectionStrategy.StrictAverage:
                default:
                    return StrictAverageSelector.Instance;
            }
        }


        public static IObservable<SingleEyeGazeData> SelectEye(this IObservable<GazeData> gazeData, ISingleEyeGazeDataSelector selector)
        {
            return selector.SelectSingleEye(gazeData);
        }


        public static IObservable<SingleEyeGazeData> SelectEye(this IObservable<GazeData> gazeData, EyeSelectionStrategy strategy)
        {
            ISingleEyeGazeDataSelector selector = ResolveSelector(strategy);

            return SelectEye(gazeData, selector);
        }


        public static IObservable<SingleEyeGazeData> SelectEye(this IObservable<GazeData> gazeData, IEyeSelectionOptions options)
        {
            return SelectEye(gazeData, options.Strategy);
        }
    }
}
