using Toggl.Core.UI.Collections;
using Toggl.Core.UI.ViewModels.TimeEntriesLog.Identity;

namespace Toggl.Core.UI.ViewModels.TimeEntriesLog
{
    public class SuggestionsSectionViewModel : MainLogSectionViewModel
    {
        public string Title { get; }

        public SuggestionsSectionViewModel(string title)
        {
            Title = title;
        }

        public override bool Equals(MainLogItemViewModel logItem)
        {
            if (ReferenceEquals(null, logItem)) return false;
            if (ReferenceEquals(this, logItem)) return true;
            if (!(logItem is SuggestionsSectionViewModel other)) return false;

            return Title == other.Title;
        }
    }
}
