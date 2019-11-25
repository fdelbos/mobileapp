using Toggl.Shared.Models.Reports;

namespace Toggl.Core.UI.ViewModels.Reports
{
    public class ReportProjectsBarChartElement : ReportBarChartElement
    {
        public ReportProjectsBarChartElement(ITimeEntriesTotals report)
            : base(convertReportTimeEntriesToBars(report), convertReportTimeEntriesToOffsets(report))
        {
        }
        
    }
}