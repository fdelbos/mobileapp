using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Toggl.Shared;

namespace Toggl.Core.UI.ViewModels.Reports
{
    public class ReportBarChartElement : CompositeReportElement
    {
        private readonly IReportElement barsElement;
        private readonly ReportBarChartHeaderElement headerElement;
        private readonly ReportBarChartFooterElement footerElement;

        public ReportBarChartElement(
            IEnumerable<Bar> bars,
            IEnumerable<DateTimeOffset> offsets,
            Func<IEnumerable<Bar>, ReportBarChartBarsElement> barsSelector = null,
            Func<IEnumerable<DateTimeOffset>, ReportBarChartFooterElement> footerSelector = null
        )
            : base(false)
        {
            headerElement = new ReportBarChartHeaderElement();

            barsSelector = barsSelector ?? defaultBarsSelector;
            barsElement = barsSelector(bars);

            footerSelector = footerSelector ?? defaultFooterSelector;
            footerElement = footerSelector(offsets);

            SubElements = ImmutableList.Create(new IReportElement[] {headerElement, barsElement, footerElement});
        }

        private ReportBarChartElement(bool isLoading)
            : base(isLoading)
        {
        }

        public static ReportBarChartElement LoadingState
            => new ReportBarChartElement(true);

        private static ReportBarChartBarsElement defaultBarsSelector(IEnumerable<Bar> bars)
            => new ReportBarChartBarsElement(bars.ToImmutableList());

        private static ReportBarChartFooterElement defaultFooterSelector(IEnumerable<DateTimeOffset> offsets)
            => new ReportBarChartFooterElement(offsets.ToImmutableList());

        public override bool Equals(IReportElement other)
            => other is ReportBarChartElement barChartElement
               && barChartElement.IsLoading == IsLoading
               && barChartElement.barsElement.Equals(barsElement)
               && barChartElement.headerElement.Equals(headerElement)
               && barChartElement.footerElement.Equals(footerElement);

        public struct Bar
        {
            public double FilledValue { get; }
            public double TransparentValue { get; }

            public Bar(double filledValue, double transparentValue)
            {
                FilledValue = filledValue;
                TransparentValue = transparentValue;
            }

            public override bool Equals(object obj)
                => obj is Bar bar
                   && bar.FilledValue == FilledValue
                   && bar.TransparentValue == TransparentValue;

            public override int GetHashCode()
                => HashCode.From(FilledValue, TransparentValue);

            public static bool operator ==(Bar left, Bar right)
                => left.Equals(right);

            public static bool operator !=(Bar left, Bar right)
                => !(left == right);
        }
    }
}