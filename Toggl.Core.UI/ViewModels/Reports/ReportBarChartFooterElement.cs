using System;
using System.Collections.Immutable;
using System.Linq;

namespace Toggl.Core.UI.ViewModels.Reports
{
    public class ReportBarChartFooterElement : ReportElementBase
    {
        public IImmutableList<DateTimeOffset> Offsets = ImmutableList<DateTimeOffset>.Empty;

        public ReportBarChartFooterElement(ImmutableList<DateTimeOffset> offsets)
            : base(false)
        {
            Offsets = offsets;
        }

        private ReportBarChartFooterElement(bool isLoading)
            : base(isLoading)
        {
        }

        public static ReportBarChartFooterElement LoadingState
            => new ReportBarChartFooterElement(true);

        public override bool Equals(IReportElement other)
            => other is ReportBarChartFooterElement reportBarChartFooterElement
               && reportBarChartFooterElement.IsLoading == IsLoading
               && reportBarChartFooterElement.Offsets.SequenceEqual(Offsets);
    }
}