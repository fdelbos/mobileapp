using Toggl.Core.UI.Collections;
using Toggl.Core.UI.ViewModels.TimeEntriesLog.Identity;

namespace Toggl.Core.UI.ViewModels.TimeEntriesLog
{
    public abstract class MainLogSectionViewModel : IDiffable<IMainLogKey>
    {
        public IMainLogKey Identity { get; protected set; }
    }
}
