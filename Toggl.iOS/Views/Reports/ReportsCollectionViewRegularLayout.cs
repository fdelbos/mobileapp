using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using Toggl.iOS.Cells.Reports;
using UIKit;

namespace Toggl.iOS.Views.Reports
{
    public class ReportsCollectionViewRegularLayout : UICollectionViewLayout
    {
        private const int maxWidth = 834;
        private const int horizontalCellInset = 8;
        private const int verticalCellInset = 12;

        private List<UICollectionViewLayoutAttributes> layoutAttributes = new List<UICollectionViewLayoutAttributes>();

        public override CGSize CollectionViewContentSize
        {
            get
            {
                var width = CollectionView.Bounds.Width;
                if (width > maxWidth)
                    width = maxWidth;
                var height = verticalCellInset * 2
                    + ReportsDonutChartCollectionViewCell.Height
                    + ReportsProjectCollectionViewCell.Height * (CollectionView.NumberOfItemsInSection(0) - 3);
                return new CGSize(width, height);
            }
        }

        public override void PrepareLayout()
        {
            var horizontalStartPoint = (CollectionView.Bounds.Width - CollectionViewContentSize.Width) / 2;
            var columnWidth = CollectionViewContentSize.Width / 2 - horizontalCellInset * 2;
            for (var i = 0; i < CollectionView.NumberOfItemsInSection(0); i++)
            {
                var indexPath = NSIndexPath.FromItemSection(i, 0);
                UICollectionViewLayoutAttributes attributes = UICollectionViewLayoutAttributes.CreateForCell(indexPath);
                switch (i)
                {
                    case 0:
                        // The reports summary cell
                        attributes.Frame = new CGRect(
                            horizontalStartPoint + horizontalCellInset,
                            verticalCellInset,
                            columnWidth,
                            ReportsSummaryCollectionViewCell.Height);
                        break;
                    case 1:
                        // The bar chart cell
                        attributes.Frame = new CGRect(
                            horizontalStartPoint + horizontalCellInset,
                            verticalCellInset * 3 + ReportsSummaryCollectionViewCell.Height,
                            columnWidth,
                            ReportsBarChartCollectionViewCell.Height);
                        break;
                    case 2:
                        // The donut chart cell
                        attributes.Frame = new CGRect(
                            horizontalStartPoint + CollectionViewContentSize.Width / 2 + horizontalCellInset,
                            verticalCellInset,
                            columnWidth,
                            ReportsDonutChartCollectionViewCell.Height);
                        break;
                    default:
                        // The project cells
                        attributes.Frame = new CGRect(
                            horizontalStartPoint + CollectionViewContentSize.Width / 2 + horizontalCellInset,
                            verticalCellInset
                                + ReportsDonutChartCollectionViewCell.Height
                                + ReportsProjectCollectionViewCell.Height * (indexPath.Item - 3),
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
