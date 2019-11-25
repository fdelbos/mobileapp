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
        DonutChartLegendItem,
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
            switch (elements[(int)indexPath.Item])
            {
                case ReportSummaryElement _:
                    cell = collectionView.DequeueReusableCell(summaryCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemBlueColor;
                    return cell;
                case ReportBarChartElement _:
                    cell = collectionView.DequeueReusableCell(barChartCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemGreenColor;
                    return cell;
                case ReportDonutChartDonutElement _:
                    cell = collectionView.DequeueReusableCell(donutChartCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemRedColor;
                    return cell;
                case ReportDonutChartLegendItemElement _:
                    cell = collectionView.DequeueReusableCell(donutChartLegendCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemYellowColor;
                    return cell;
                case ReportNoDataElement _:
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
            switch (elements[(int)indexPath.Item])
            {
                case ReportSummaryElement _:
                    return ReportsCollectionViewCell.Summary;
                case ReportBarChartElement _:
                    return ReportsCollectionViewCell.BarChart;
                case ReportDonutChartDonutElement _:
                    return ReportsCollectionViewCell.DonutChart;
                case ReportDonutChartLegendItemElement _:
                    return ReportsCollectionViewCell.DonutChartLegendItem;
                case ReportNoDataElement _:
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
