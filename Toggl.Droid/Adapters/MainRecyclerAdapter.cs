using Android.Views;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Toggl.Core;
using Toggl.Core.Analytics;
using Toggl.Core.UI.ViewModels;
using Toggl.Core.UI.ViewModels.TimeEntriesLog;
using Toggl.Droid.ViewHelpers;
using Toggl.Droid.ViewHolders.MainLog;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Toggl.Core.UI.Collections;
using Toggl.Core.UI.Helper;
using Toggl.Core.UI.ViewModels.TimeEntriesLog.Identity;
using Toggl.Droid.ViewHolders;
using Toggl.Shared.Extensions;

namespace Toggl.Droid.Adapters
{
    using MainLogSection = AnimatableSectionModel<MainLogSectionViewModel, MainLogItemViewModel, IMainLogKey>;

    public class MainRecyclerAdapter : BaseRecyclerAdapter<MainLogItemViewModel>
    {
        public const int LogItemViewType = 1;
        public const int SuggestionItemViewType = 2;
        public const int DaySummarySectionViewType = 3;
        public const int SuggestionSectionViewType = 4;
        public const int UserFeedbackViewType = 5;

        private readonly Context context;
        private readonly ITimeService timeService;

        private bool isRatingViewVisible = false;

        public IObservable<GroupId> ToggleGroupExpansion
            => toggleGroupExpansionSubject.AsObservable();

        public IObservable<TimeEntryLogItemViewModel> TimeEntryTaps
            => timeEntryTappedSubject
                .Do(a => Log.Debug("TimeEntryTaps", a.ToString()))
                .Select(t => t as TimeEntryLogItemViewModel).AsObservable();

        public IObservable<SuggestionLogItemViewModel> SuggestionTaps
            => continueSuggestionTimeEntrySubject
                .Do(a => Log.Debug("SuggestionTaps", a.ToString()))
                .Select(t => t as SuggestionLogItemViewModel).AsObservable();

        public IObservable<ContinueTimeEntryInfo> ContinueTimeEntry
            => continueTimeEntrySubject.AsObservable();

        public IObservable<TimeEntryLogItemViewModel> DeleteTimeEntrySubject
            => deleteTimeEntrySubject.AsObservable();

        private readonly Subject<GroupId> toggleGroupExpansionSubject = new Subject<GroupId>();
        private readonly Subject<TimeEntryLogItemViewModel> deleteTimeEntrySubject = new Subject<TimeEntryLogItemViewModel>();
        private readonly Subject<MainLogItemViewModel> timeEntryTappedSubject = new Subject<MainLogItemViewModel>();
        private readonly Subject<ContinueTimeEntryInfo> continueTimeEntrySubject = new Subject<ContinueTimeEntryInfo>();
        private readonly Subject<MainLogItemViewModel> continueSuggestionTimeEntrySubject = new Subject<MainLogItemViewModel>();

        public MainRecyclerAdapter(Context context, ITimeService timeService)
        {
            this.context = context;
            this.timeService = timeService;
        }

        public MainRecyclerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override int GetItemViewType(int position)
        {
            var item = GetItem(position);
            switch (item)
            {
                case TimeEntryLogItemViewModel _:
                    return LogItemViewType;
                case SuggestionLogItemViewModel _:
                    return SuggestionItemViewType;
                case DaySummaryViewModel _:
                    return DaySummarySectionViewType;
                case SuggestionsSectionViewModel _:
                    return SuggestionSectionViewType;
                case UserFeedbackViewModel _:
                    return UserFeedbackViewType;
                default:
                    throw new Exception("Invalid item type");
            }
        }

        protected override BaseRecyclerViewHolder<MainLogItemViewModel> CreateViewHolder(ViewGroup parent,
            LayoutInflater inflater, int viewType)
        {
            switch (viewType)
            {
                case LogItemViewType:
                    var logItemView = LayoutInflater.FromContext(parent.Context)
                        .Inflate(Resource.Layout.MainLogCell, parent, false);
                    var mainLogCellViewHolder = new MainLogCellViewHolder(logItemView)
                    {
                        TappedSubject = timeEntryTappedSubject,
                        ContinueButtonTappedSubject = continueTimeEntrySubject,
                        ToggleGroupExpansionSubject = toggleGroupExpansionSubject
                    };
                    return mainLogCellViewHolder;
                case SuggestionItemViewType:
                    var suggestionsView = LayoutInflater.FromContext(parent.Context)
                        .Inflate(Resource.Layout.MainSuggestionsCard, parent, false);
                    var mainLogSuggestionItemViewHolder = new MainLogSuggestionItemViewHolder(suggestionsView)
                    {
                        TappedSubject = continueSuggestionTimeEntrySubject
                    };
                    return mainLogSuggestionItemViewHolder;
                case DaySummarySectionViewType:
                    var sectionView = LayoutInflater.FromContext(parent.Context)
                        .Inflate(Resource.Layout.MainLogHeader, parent, false);
                    return new MainLogSectionViewHolder(sectionView);
                case SuggestionSectionViewType:
                    var suggestionsSectionView = LayoutInflater.FromContext(parent.Context)
                        .Inflate(Resource.Layout.MainLogSuggestionsHeader, parent, false);
                    return new MainLogSuggestionSectionViewHolder(suggestionsSectionView);
                case UserFeedbackViewType:
                    var userFeedbackView = LayoutInflater.FromContext(parent.Context).Inflate(Resource.Layout.MainUserFeedbackCard, parent, false);
                    return new MainLogUserFeedbackViewHolder(userFeedbackView);
                default:
                    throw new Exception("Invalid view type");
            }
        }

        public void UpdateCollection(IImmutableList<MainLogSection> items)
        {
            var flattenItems = items.Aggregate(ImmutableList<MainLogItemViewModel>.Empty, (acc, nextSection) => acc.AddRange(nextSection.Items.Prepend(nextSection.Header)));
            SetItems(flattenItems);
        }

        protected override void SetItems(IImmutableList<MainLogItemViewModel> newItems)
        {

            if (isRatingViewVisible && Items.Count > 0)
            {
                var ratingIndex = newItems.Select((value, index) => new {index, value})
                    .Single(p => p.value is DaySummaryViewModel)?.index ?? 0;
                base.SetItems(newItems.Insert(ratingIndex, new UserFeedbackViewModel()));
            }
            else
            {
                base.SetItems(newItems);
            }
        }

        public void ContinueTimeEntryBySwiping(int position)
        {
            var continuedTimeEntry = GetItem(position) as TimeEntryLogItemViewModel;
            NotifyItemChanged(position);

            var continueMode = continuedTimeEntry.IsTimeEntryGroupHeader
                ? ContinueTimeEntryMode.TimeEntriesGroupSwipe
                : ContinueTimeEntryMode.SingleTimeEntrySwipe;

            continueTimeEntrySubject.OnNext(new ContinueTimeEntryInfo(continuedTimeEntry, continueMode));
        }

        public void DeleteTimeEntry(int position)
        {
            var deletedTimeEntry = GetItem(position) as TimeEntryLogItemViewModel;
            deleteTimeEntrySubject.OnNext(deletedTimeEntry);
        }

        public void SetupRatingViewVisibility(bool isVisible)
        {
            if (isRatingViewVisible == isVisible)
                return;

            isRatingViewVisible = isVisible;
            SetItems(Items);
        }
    }
}
