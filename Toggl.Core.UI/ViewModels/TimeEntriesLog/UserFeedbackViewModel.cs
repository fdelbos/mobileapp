using Toggl.Core.UI.Collections;
using Toggl.Core.UI.ViewModels.TimeEntriesLog.Identity;

namespace Toggl.Core.UI.ViewModels.TimeEntriesLog
{
    public class UserFeedbackViewModel : MainLogItemViewModel
    {
        public override bool Equals(MainLogItemViewModel logItem)
        {
            if (ReferenceEquals(null, logItem)) return false;
            if (ReferenceEquals(this, logItem)) return true;
            return logItem is SuggestionsSectionViewModel other;
        }
    }
}
