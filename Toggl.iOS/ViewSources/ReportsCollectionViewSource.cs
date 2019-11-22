using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Foundation;
using Toggl.Core.UI.ViewModels.Reports;
using Toggl.iOS.Cells.Reports;
using UIKit;

namespace Toggl.iOS.ViewSources
{
    public enum ReportsCollectionViewCell
    {
        Summary,
        BarChart,
        DonutChart,
        DonutChartLegend,
        Error,
        NoData
    }

    public class ReportsCollectionViewSource : UICollectionViewSource
    {
        private readonly UICollectionView collectionView;

        private IImmutableList<IReportElement> elements;

        private const string summaryCellIdentifier = nameof(summaryCellIdentifier);
        private const string barChartCellIdentifier = nameof(barChartCellIdentifier);
        private const string donutChartCellIdentifier = nameof(donutChartCellIdentifier);
        private const string donutChartLegendCellIdentifier = nameof(donutChartLegendCellIdentifier);
        private const string noDataCellIdentifier = nameof(noDataCellIdentifier);
        private const string errorCellIdentifier = nameof(errorCellIdentifier);
        private const string workspaceCellIdentifier = nameof(workspaceCellIdentifier);

        public ReportsCollectionViewSource(UICollectionView collectionView)
        {
            this.collectionView = collectionView;

            collectionView.RegisterNibForCell(ReportsSummaryCollectionViewCell.Nib, summaryCellIdentifier);
            collectionView.RegisterNibForCell(ReportsBarChartCollectionViewCell.Nib, barChartCellIdentifier);
            collectionView.RegisterNibForCell(ReportsDonutChartCollectionViewCell.Nib, donutChartCellIdentifier);
            collectionView.RegisterNibForCell(ReportsDonutChartLegendCollectionViewCell.Nib, donutChartLegendCellIdentifier);
            collectionView.RegisterNibForCell(ReportsNoDataCollectionViewCell.Nib, noDataCellIdentifier);
            collectionView.RegisterNibForCell(ReportsErrorCollectionViewCell.Nib, errorCellIdentifier);
        }

        public void SetNewElements(IImmutableList<IReportElement> elements)
        {
            this.elements = elements.Where(e => e.GetType().Name != nameof(ReportWorkspaceNameElement)).ToImmutableList();
            collectionView.ReloadData();
            collectionView.CollectionViewLayout.InvalidateLayout();
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            UICollectionViewCell cell;
            switch (elements[(int)indexPath.Item].GetType().Name)
            {
                case nameof(ReportSummaryElement):
                    cell = collectionView.DequeueReusableCell(summaryCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemBlueColor;
                    return cell;
                case nameof(ReportBarChartElement):
                    cell = collectionView.DequeueReusableCell(barChartCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemGreenColor;
                    return cell;
                case nameof(ReportDonutChartDonutElement):
                    cell = collectionView.DequeueReusableCell(donutChartCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemRedColor;
                    return cell;
                case nameof(ReportDonutChartLegendItemElement):
                    cell = collectionView.DequeueReusableCell(donutChartLegendCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemYellowColor;
                    return cell;
                case nameof(ReportNoDataElement):
                    cell = collectionView.DequeueReusableCell(noDataCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemPurpleColor;
                    return cell;
                default:
                    cell = collectionView.DequeueReusableCell(errorCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemTealColor;
                    return cell;
            }
        }

        public override nint NumberOfSections(UICollectionView collectionView)
            => 1;

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
            => elements == null ? 0 : elements.Count();

        public ReportsCollectionViewCell CellTypeAt(NSIndexPath indexPath)
        {
            switch (elements[(int)indexPath.Item].GetType().Name)
            {
                case nameof(ReportSummaryElement):
                    return ReportsCollectionViewCell.Summary;
                case nameof(ReportBarChartElement):
                    return ReportsCollectionViewCell.BarChart;
                case nameof(ReportDonutChartDonutElement):
                    return ReportsCollectionViewCell.DonutChart;
                case nameof(ReportDonutChartLegendItemElement):
                    return ReportsCollectionViewCell.DonutChartLegend;
                case nameof(ReportNoDataElement):
                    return ReportsCollectionViewCell.NoData;
                default:
                    return ReportsCollectionViewCell.Error;
            }
        }

        public int NumberOfDonutChartLegendItems()
        {
            return elements == null
                ? 0
                : elements.Where(e => e.GetType().Name == nameof(ReportDonutChartLegendItemElement)).Count();
        }

        public bool HasDataToDisplay()
            => elements != null && elements.Count > 0;
    }
}
