using Toggl.Core.UI.Collections;
using Toggl.Core.UI.ViewModels.TimeEntriesLog.Identity;

namespace Toggl.Core.UI.ViewModels.TimeEntriesLog
{
    public class SuggestionsSectionViewModel : MainLogSectionViewModel
    {
        public SuggestionsSectionViewModel()
        {
            Identity = new SuggestionsSectionKey();
        }
    }
}
