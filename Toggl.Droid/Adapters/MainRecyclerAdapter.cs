using Android.Support.V7.Widget;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Toggl.Core;
using Toggl.Core.Analytics;
using Toggl.Core.UI.ViewModels;
using Toggl.Core.UI.ViewModels.TimeEntriesLog;
using Toggl.Core.UI.ViewModels.TimeEntriesLog.Identity;
using Toggl.Droid.ViewHelpers;
using Toggl.Droid.ViewHolders;
using Toggl.Shared.Extensions;
using Android.Content;
using Toggl.Core.UI.Helper;

namespace Toggl.Droid.Adapters
{
    public class MainRecyclerAdapter : ReactiveSectionedRecyclerAdapter<MainLogItemViewModel, TimeEntryViewData, DaySummaryViewModel, DaySummaryViewModel, MainLogCellViewHolder, MainLogSectionViewHolder, IMainLogKey>
    {
        public const int SuggestionViewType = 2;
        public const int UserFeedbackViewType = 3;

        private readonly Context context;
        private readonly ITimeService timeService;

        private bool isRatingViewVisible = false;

        public IObservable<GroupId> ToggleGroupExpansion
            => toggleGroupExpansionSubject.AsObservable();

        public IObservable<TimeEntryLogItemViewModel> TimeEntryTaps
            => timeEntryTappedSubject.Select(item => item.ViewModel).AsObservable();

        public IObservable<ContinueTimeEntryInfo> ContinueTimeEntry
            => continueTimeEntrySubject.AsObservable();

        public IObservable<TimeEntryLogItemViewModel> DeleteTimeEntrySubject
            => deleteTimeEntrySubject.AsObservable();

        public SuggestionsViewModel SuggestionsViewModel { get; set; }
        public RatingViewModel RatingViewModel { get; set; }

        private readonly Subject<GroupId> toggleGroupExpansionSubject = new Subject<GroupId>();
        private readonly Subject<TimeEntryLogItemViewModel> deleteTimeEntrySubject = new Subject<TimeEntryLogItemViewModel>();
        private readonly Subject<TimeEntryViewData> timeEntryTappedSubject = new Subject<TimeEntryViewData>();
        private readonly Subject<ContinueTimeEntryInfo> continueTimeEntrySubject = new Subject<ContinueTimeEntryInfo>();

        public MainRecyclerAdapter(Context context, ITimeService timeService)
        {
            this.context = context;
            this.timeService = timeService;
        }

        public void ContinueTimeEntryBySwiping(int position)
        {
            var continuedTimeEntry = GetItemAt(position) as TimeEntryLogItemViewModel;
            NotifyItemChanged(position);

            var continueMode = continuedTimeEntry.IsTimeEntryGroupHeader
                ? ContinueTimeEntryMode.TimeEntriesGroupSwipe
                : ContinueTimeEntryMode.SingleTimeEntrySwipe;

            continueTimeEntrySubject.OnNext(new ContinueTimeEntryInfo(continuedTimeEntry, continueMode));
        }

        public void DeleteTimeEntry(int position)
        {
            var deletedTimeEntry = GetItemAt(position) as TimeEntryLogItemViewModel;
            deleteTimeEntrySubject.OnNext(deletedTimeEntry);
        }

        public override int HeaderOffset => isRatingViewVisible ? 2 : 1;

        protected override bool TryBindCustomViewType(RecyclerView.ViewHolder holder, int position)
        {
            return holder is MainLogSuggestionsListViewHolder
                || holder is MainLogUserFeedbackViewHolder;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == SuggestionViewType)
            {
                var suggestionsView = LayoutInflater.FromContext(parent.Context).Inflate(Resource.Layout.MainSuggestions, parent, false);
                var mainLogSuggestionsListViewHolder = new MainLogSuggestionsListViewHolder(suggestionsView, SuggestionsViewModel);
                return mainLogSuggestionsListViewHolder;
            }

            if (viewType == UserFeedbackViewType)
            {
                var suggestionsView = LayoutInflater.FromContext(parent.Context).Inflate(Resource.Layout.MainUserFeedbackCard, parent, false);
                var userFeedbackViewHolder = new MainLogUserFeedbackViewHolder(suggestionsView, RatingViewModel);
                return userFeedbackViewHolder;
            }

            return base.OnCreateViewHolder(parent, viewType);
        }

        public override int GetItemViewType(int position)
        {
            if (position == 0)
                return SuggestionViewType;

            if (isRatingViewVisible && position == 1)
                return UserFeedbackViewType;

            return base.GetItemViewType(position);
        }

        protected override MainLogSectionViewHolder CreateHeaderViewHolder(ViewGroup parent)
            => new MainLogSectionViewHolder(LayoutInflater.FromContext(parent.Context)
                .Inflate(Resource.Layout.MainLogHeader, parent, false));

        public void SetupRatingViewVisibility(bool isVisible)
        {
            if (isRatingViewVisible == isVisible)
                return;

            isRatingViewVisible = isVisible;
            NotifyDataSetChanged();
        }

        protected override MainLogCellViewHolder CreateItemViewHolder(ViewGroup parent)
        {
            var mainLogCellViewHolder = new MainLogCellViewHolder(LayoutInflater.FromContext(parent.Context).Inflate(Resource.Layout.MainLogCell, parent, false))
            {
                TappedSubject = timeEntryTappedSubject,
                ContinueButtonTappedSubject = continueTimeEntrySubject,
                ToggleGroupExpansionSubject = toggleGroupExpansionSubject
            };

            return mainLogCellViewHolder;
        }

        protected override IMainLogKey IdFor(MainLogItemViewModel item)
            => item.Identity;

        protected override IMainLogKey IdForSection(DaySummaryViewModel section)
            => section.Identity;

        protected override TimeEntryViewData Wrap(MainLogItemViewModel item)
            => new TimeEntryViewData(context, item as TimeEntryLogItemViewModel);

        protected override DaySummaryViewModel Wrap(DaySummaryViewModel section)
            => section;

        protected override bool AreItemContentsTheSame(MainLogItemViewModel item1, MainLogItemViewModel item2)
            => item1 == item2;

        protected override bool AreSectionsRepresentationsTheSame(
            DaySummaryViewModel oneHeader,
            DaySummaryViewModel otherHeader,
            IReadOnlyList<MainLogItemViewModel> one,
            IReadOnlyList<MainLogItemViewModel> other)
        {
            return oneHeader.Title == otherHeader.Title && one.ContainsExactlyAll(other);
        }
    }
}
