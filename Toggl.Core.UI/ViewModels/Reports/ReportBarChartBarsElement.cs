using System.Collections.Immutable;
using System.Linq;
using Bar = Toggl.Core.UI.ViewModels.Reports.ReportBarChartElement.Bar;

namespace Toggl.Core.UI.ViewModels.Reports
{
    public class ReportBarChartBarsElement : ReportElementBase
    {
        public ImmutableList<Bar> Bars { get; } = ImmutableList<Bar>.Empty;

        public ReportBarChartBarsElement(ImmutableList<Bar> bars)
            : base(false)
        {
            Bars = bars;
        }

        private ReportBarChartBarsElement(bool isLoading)
            : base(isLoading)
        {
        }

        public static ReportBarChartBarsElement LoadingState
            => new ReportBarChartBarsElement(true);

        public override bool Equals(IReportElement other)
            => other is ReportBarChartBarsElement barChartBarsElement
               && barChartBarsElement.IsLoading == IsLoading
               && barChartBarsElement.Bars.SequenceEqual(Bars);
    }
}
