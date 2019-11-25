using System;
using System.Collections.Immutable;
using System.Linq;
using Toggl.Shared;
using Toggl.Shared.Models.Reports;
using static System.Math;


namespace Toggl.Core.UI.ViewModels.Reports
{
    public class ReportProjectsBarChartElement : ReportBarChartElement
    {
        private const int maximumLabeledNumberOfDays = 7;
        private const int roundToMultiplesOf = 2;

        public ReportProjectsBarChartElement(ITimeEntriesTotals report)
            : base(convertToBars(report), convertToOffsets(report), null, null)
        {
        }

        private static IImmutableList<Bar> convertToBars(ITimeEntriesTotals report)
        {
            var upperLimit = upperHoursLimit(report);
            return report.Groups.Select(normalizedBar(upperLimit)).ToImmutableList();
        }

        private static int upperHoursLimit(ITimeEntriesTotals report)
        {
            var maximumTotalTrackedTimePerGroup = report.Groups.Max(group => group.Total);
            var rounded = (int) Ceiling(maximumTotalTrackedTimePerGroup.TotalHours / roundToMultiplesOf) * roundToMultiplesOf;
            
            return Max(roundToMultiplesOf, rounded);
        }

        private static Func<ITimeEntriesTotalsGroup, Bar> normalizedBar(double maxHours)
            => group =>
            {
                var billableHours = group.Billable.TotalHours;
                var nonBillableHours = group.Total.TotalHours - billableHours;
                
                return new Bar(billableHours / maxHours, nonBillableHours / maxHours);
            };

        private static IImmutableList<DateTimeOffset> convertToOffsets(ITimeEntriesTotals report)
            => report.Groups.Length <= maximumLabeledNumberOfDays && report.Resolution == Resolution.Day
                ? daysRange(report.Groups.Length, report.StartDate)
                : ImmutableList<DateTimeOffset>.Empty;

        private static IImmutableList<DateTimeOffset> daysRange(int numberOfDays, DateTimeOffset startDate)
            => Enumerable.Range(0, numberOfDays)
                .Select(i => startDate.AddDays(i))
                .ToImmutableList();
    }
}