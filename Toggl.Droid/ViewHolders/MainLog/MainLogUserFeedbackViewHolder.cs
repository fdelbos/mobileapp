using Android.Runtime;
using Android.Support.Constraints;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Toggl.Core.UI.Extensions;
using Toggl.Core.UI.ViewModels;
using Toggl.Core.UI.ViewModels.TimeEntriesLog;
using Toggl.Droid.Extensions.Reactive;
using Toggl.Shared.Extensions;
using static Toggl.Shared.Extensions.CommonFunctions;

namespace Toggl.Droid.ViewHolders.MainLog
{
    public class MainLogUserFeedbackViewHolder : BaseRecyclerViewHolder<MainLogItemViewModel>
    {
        private TextView userFeedbackTitle;
        private ImageView thumbsUpButton;
        private ImageView thumbsDownButton;
        private TextView yesText;
        private TextView noText;
        private TextView impressionTitle;
        private ImageView impressionThumbsImage;
        private TextView impressionDescription;
        private Button rateButton;
        private TextView laterButton;

        private Group questionGroup;
        private Group impressionGroup;

        private readonly CompositeDisposable disposeBag = new CompositeDisposable();

        public MainLogUserFeedbackViewHolder(View itemView) : base(itemView)
        {
        }

        public MainLogUserFeedbackViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void InitializeViews()
        {
            userFeedbackTitle = ItemView.FindViewById<TextView>(Resource.Id.UserFeedbackTitle);
            thumbsUpButton = ItemView.FindViewById<ImageView>(Resource.Id.UserFeedbackThumbsUp);
            thumbsDownButton = ItemView.FindViewById<ImageView>(Resource.Id.UserFeedbackThumbsDown);
            yesText = ItemView.FindViewById<TextView>(Resource.Id.UserFeedbackThumbsUpText);
            noText = ItemView.FindViewById<TextView>(Resource.Id.UserFeedbackThumbsDownText);
            impressionTitle = ItemView.FindViewById<TextView>(Resource.Id.UserFeedbackImpressionTitle);
            impressionThumbsImage = ItemView.FindViewById<ImageView>(Resource.Id.UserFeedbackImpressionThumbsImage);
            impressionDescription = ItemView.FindViewById<TextView>(Resource.Id.UserFeedbackDescription);
            rateButton = ItemView.FindViewById<Button>(Resource.Id.UserFeedbackRateButton);
            laterButton = ItemView.FindViewById<TextView>(Resource.Id.UserFeedbackLaterButton);

            questionGroup = ItemView.FindViewById<Group>(Resource.Id.QuestionView);
            impressionGroup = ItemView.FindViewById<Group>(Resource.Id.ImpressionView);

            userFeedbackTitle.Text = Shared.Resources.RatingTitle;
            yesText.Text = Shared.Resources.RatingYes;
            noText.Text = Shared.Resources.RatingNotReally;
            laterButton.Text = Shared.Resources.Later;

        }

        protected override void UpdateView()
        {
            // TODO
//            Item.Impression
//                .Select(callToActionTitle)
//                .Subscribe(impressionTitle.Rx().TextObserver())
//                .DisposedBy(disposeBag);
//
//            Item.Impression
//                .Select(callToActionDescription)
//                .Subscribe(impressionDescription.Rx().TextObserver())
//                .DisposedBy(disposeBag);
//
//            Item.Impression
//                .Select(callToActionButtonTitle)
//                .Subscribe(rateButton.Rx().TextObserver())
//                .DisposedBy(disposeBag);
//
//            Item.Impression
//                .Select(impression => impression.HasValue)
//                .Subscribe(impressionGroup.Rx().IsVisible())
//                .DisposedBy(disposeBag);
//
//            Item.Impression
//                .Select(impression => impression.HasValue)
//                .Select(Invert)
//                .Subscribe(questionGroup.Rx().IsVisible())
//                .DisposedBy(disposeBag);
//
//            Item.Impression
//               .Select(impression => impression ?? false)
//               .Select(drawableFromImpression)
//               .Subscribe(impressionThumbsImage.Rx().Image(itemView.Context))
//               .DisposedBy(disposeBag);
//
//            thumbsUpButton.Rx().Tap()
//                .Subscribe(() => Item.RegisterImpression(true))
//                .DisposedBy(disposeBag);
//
//            yesText.Rx().Tap()
//                .Subscribe(() => Item.RegisterImpression(true))
//                .DisposedBy(disposeBag);
//
//            thumbsDownButton.Rx().Tap()
//                .Subscribe(() => Item.RegisterImpression(false))
//                .DisposedBy(disposeBag);
//
//            noText.Rx().Tap()
//                .Subscribe(() => Item.RegisterImpression(false))
//                .DisposedBy(disposeBag);
//
//            rateButton.Rx().Tap()
//                .Subscribe(Item.PerformMainAction.Inputs)
//                .DisposedBy(disposeBag);
//
//            laterButton.Rx().Tap()
//                .Subscribe(Item.Dismiss)
//                .DisposedBy(disposeBag);
        }

        private int drawableFromImpression(bool impression)
            => impression ? Resource.Drawable.ic_thumbs_up : Resource.Drawable.ic_thumbs_down;

        private string callToActionTitle(bool? impressionIsPositive)
        {
            if (impressionIsPositive == null)
                return string.Empty;

            return impressionIsPositive.Value
                   ? Shared.Resources.RatingViewPositiveCallToActionTitle
                   : Shared.Resources.RatingViewNegativeCallToActionTitle;
        }

        private string callToActionDescription(bool? impressionIsPositive)
        {
            if (impressionIsPositive == null)
                return string.Empty;

            return impressionIsPositive.Value
                   ? Shared.Resources.RatingViewPositiveCallToActionDescriptionDroid
                   : Shared.Resources.RatingViewNegativeCallToActionDescription;
        }

        private string callToActionButtonTitle(bool? impressionIsPositive)
        {
            if (impressionIsPositive == null)
                return string.Empty;

            return impressionIsPositive.Value
                   ? Shared.Resources.RatingViewPositiveCallToActionButtonTitle
                   : Shared.Resources.RatingViewNegativeCallToActionButtonTitle;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;

            disposeBag.Dispose();
        }
    }
}
