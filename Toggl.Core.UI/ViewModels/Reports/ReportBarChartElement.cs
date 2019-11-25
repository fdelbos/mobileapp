using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Toggl.Shared;
using Toggl.Shared.Models.Reports;
using static System.Math;


namespace Toggl.Core.UI.ViewModels.Reports
{
    public class ReportBarChartElement : ReportElementBase
    {
        private const int maximumLabeledNumberOfDays = 7;
        private const int roundToMultiplesOf = 2;

        public ImmutableList<Bar> Bars { get; } = ImmutableList<Bar>.Empty;
        public ImmutableList<DateTimeOffset> HorizontalLegendItems { get; } = ImmutableList<DateTimeOffset>.Empty;

        public ReportBarChartElement(
            IEnumerable<Bar> bars,
            IEnumerable<DateTimeOffset> horizontalLegendItems
        )
            : base(false)
        {
            Bars = bars.ToImmutableList();
            HorizontalLegendItems = horizontalLegendItems.ToImmutableList();
        }

        private ReportBarChartElement(bool isLoading)
            : base(isLoading)
        {
        }

        public static ReportBarChartElement LoadingState
            => new ReportBarChartElement(true);

        protected static IImmutableList<Bar> convertReportTimeEntriesToBars(ITimeEntriesTotals report)
        {
            var upperLimit = upperHoursLimit(report);
            return report.Groups.Select(normalizedBar(upperLimit)).ToImmutableList();
        }

        private static int upperHoursLimit(ITimeEntriesTotals report)
        {
            var maximumTotalTrackedTimePerGroup = report.Groups.Max(group => group.Total);
            var rounded = (int) Ceiling(maximumTotalTrackedTimePerGroup.TotalHours / roundToMultiplesOf) *
                          roundToMultiplesOf;

            return Max(roundToMultiplesOf, rounded);
        }

        private static Func<ITimeEntriesTotalsGroup, Bar> normalizedBar(double maxHours)
            => group =>
            {
                var billableHours = group.Billable.TotalHours;
                var nonBillableHours = group.Total.TotalHours - billableHours;

                return new Bar(billableHours / maxHours, nonBillableHours / maxHours);
            };

        protected static IImmutableList<DateTimeOffset> convertReportTimeEntriesToOffsets(ITimeEntriesTotals report)
            => report.Groups.Length <= maximumLabeledNumberOfDays && report.Resolution == Resolution.Day
                ? daysRange(report.Groups.Length, report.StartDate)
                : ImmutableList<DateTimeOffset>.Empty;

        private static IImmutableList<DateTimeOffset> daysRange(int numberOfDays, DateTimeOffset startDate)
            => Enumerable.Range(0, numberOfDays)
                .Select(i => startDate.AddDays(i))
                .ToImmutableList();

        public override bool Equals(IReportElement other)
            => other is ReportBarChartElement barChartElement
               && barChartElement.IsLoading == IsLoading
               && barChartElement.Bars.SequenceEqual(Bars)
               && barChartElement.HorizontalLegendItems.SequenceEqual(HorizontalLegendItems);

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