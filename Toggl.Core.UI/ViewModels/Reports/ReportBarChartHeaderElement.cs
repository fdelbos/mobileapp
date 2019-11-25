namespace Toggl.Core.UI.ViewModels.Reports
{
    public class ReportBarChartHeaderElement : ReportElementBase
    {
        public override bool Equals(IReportElement other)
            => other is ReportBarChartHeaderElement reportBarChartHeaderElement
               && reportBarChartHeaderElement.IsLoading == IsLoading;
    }
}