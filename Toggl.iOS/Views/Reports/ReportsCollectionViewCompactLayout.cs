using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using Toggl.iOS.Cells.Reports;
using UIKit;

namespace Toggl.iOS.Views.Reports
{
    public class ReportsCollectionViewCompactLayout : UICollectionViewLayout
    {
        private const int horizontalCellInset = 16;
        private const int verticalCellInset = 12;
        private const int numberOfNonProjectItems = 3;

        private List<UICollectionViewLayoutAttributes> layoutAttributes = new List<UICollectionViewLayoutAttributes>();

        public override CGSize CollectionViewContentSize
        {
            get
            {
                var width = CollectionView.Bounds.Width;
                var height = verticalCellInset * 6
                    + ReportsSummaryCollectionViewCell.Height
                    + ReportsBarChartCollectionViewCell.Height
                    + ReportsDonutChartCollectionViewCell.Height
                    + ReportsProjectCollectionViewCell.Height * (CollectionView.NumberOfItemsInSection(0) - numberOfNonProjectItems);
                return new CGSize(width, height);
            }
        }

        public override void PrepareLayout()
        {
            var columnWidth = CollectionViewContentSize.Width - horizontalCellInset * 2;
            for (var i = 0; i < CollectionView.NumberOfItemsInSection(0); i++)
            {
                var indexPath = NSIndexPath.FromItemSection(i, 0);
                UICollectionViewLayoutAttributes attributes = UICollectionViewLayoutAttributes.CreateForCell(indexPath);
                switch (i)
                {
                    case 0:
                        // The reports summary cell
                        attributes.Frame = new CGRect(
                            horizontalCellInset,
                            verticalCellInset,
                            columnWidth,
                            ReportsSummaryCollectionViewCell.Height);
                        break;
                    case 1:
                        // The bar chart cell
                        attributes.Frame = new CGRect(
                            horizontalCellInset,
                            verticalCellInset * 3 + ReportsSummaryCollectionViewCell.Height,
                            columnWidth,
                            ReportsBarChartCollectionViewCell.Height);
                        break;
                    case 2:
                        // The donut chart cell
                        attributes.Frame = new CGRect(
                            horizontalCellInset,
                            verticalCellInset * 5 + ReportsSummaryCollectionViewCell.Height + ReportsBarChartCollectionViewCell.Height,
                            columnWidth,
                            ReportsDonutChartCollectionViewCell.Height);
                        break;
                    default:
                        // The project cells
                        attributes.Frame = new CGRect(
                            horizontalCellInset,
                            verticalCellInset * 5
                                + ReportsSummaryCollectionViewCell.Height
                                + ReportsBarChartCollectionViewCell.Height
                                + ReportsDonutChartCollectionViewCell.Height
                                + ReportsProjectCollectionViewCell.Height * (indexPath.Item - numberOfNonProjectItems),
                            columnWidth,
                            ReportsProjectCollectionViewCell.Height);
                        break;
                }
                layoutAttributes.Add(attributes);
            }
        }

        public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
            => true;

        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
        {
            var visibleAttributes = new List<UICollectionViewLayoutAttributes>();
            foreach (var attributes in layoutAttributes)
            {
                if (attributes.Frame.IntersectsWith(rect))
                    visibleAttributes.Add(attributes);
            }
            return visibleAttributes.ToArray();
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath)
            => layoutAttributes[(int)indexPath.Item];
    }
}
