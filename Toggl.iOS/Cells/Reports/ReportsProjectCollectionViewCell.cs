using System;

using Foundation;
using UIKit;

namespace Toggl.iOS.Cells.Reports
{
    public partial class ReportsProjectCollectionViewCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString("ReportsProjectCollectionViewCell");
        public static readonly UINib Nib;
        public static readonly int Height = 56;

        static ReportsProjectCollectionViewCell()
        {
            Nib = UINib.FromName("ReportsProjectCollectionViewCell", NSBundle.MainBundle);
        }

        protected ReportsProjectCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}

