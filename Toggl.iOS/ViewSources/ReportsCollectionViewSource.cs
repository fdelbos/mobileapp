using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Foundation;
using Toggl.Core.UI.ViewModels.Reports;
using Toggl.iOS.Cells.Reports;
using UIKit;

namespace Toggl.iOS.ViewSources
{
    public class ReportsCollectionViewSource : UICollectionViewSource
    {
        private readonly UICollectionView collectionView;

        private IImmutableList<IReportElement> elements;

        private const string summaryCellIdentifier = nameof(summaryCellIdentifier);
        private const string barChartCellIdentifier = nameof(barChartCellIdentifier);
        private const string donutChartCellIdentifier = nameof(donutChartCellIdentifier);
        private const string donutChartLegendCellIdentifier = nameof(donutChartLegendCellIdentifier);

        public ReportsCollectionViewSource(UICollectionView collectionView)
        {
            this.collectionView = collectionView;

            collectionView.RegisterNibForCell(ReportsSummaryCollectionViewCell.Nib, summaryCellIdentifier);
            collectionView.RegisterNibForCell(ReportsBarChartCollectionViewCell.Nib, barChartCellIdentifier);
            collectionView.RegisterNibForCell(ReportsDonutChartCollectionViewCell.Nib, donutChartCellIdentifier);
            collectionView.RegisterNibForCell(ReportsDonutChartLegendCollectionViewCell.Nib, donutChartLegendCellIdentifier);
        }

        public void SetNewElements(IImmutableList<IReportElement> elements)
        {
            this.elements = elements;
            collectionView.ReloadData();
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            UICollectionViewCell cell;
            switch (indexPath.Item)
            {
                case 0:
                    cell = collectionView.DequeueReusableCell(summaryCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemBlueColor;
                    return cell;
                case 1:
                    cell = collectionView.DequeueReusableCell(barChartCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemGreenColor;
                    return cell;
                case 2:
                    cell = collectionView.DequeueReusableCell(donutChartCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemRedColor;
                    return cell;
                default:
                    cell = collectionView.DequeueReusableCell(donutChartLegendCellIdentifier, indexPath) as UICollectionViewCell;
                    // TODO: populate cell
                    cell.BackgroundColor = UIColor.SystemYellowColor;
                    return cell;
            }
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            // TODO: replace with actual elements count
            return 33;
        }
    }
}
