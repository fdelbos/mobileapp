using System;
using Toggl.Core.UI.Collections;
using Toggl.Core.UI.ViewModels.TimeEntriesLog.Identity;

namespace Toggl.Core.UI.ViewModels.TimeEntriesLog
{
    public abstract class MainLogItemViewModel : IDiffable<IMainLogKey>, IEquatable<MainLogItemViewModel>
    {
        public IMainLogKey Identity { get; protected set; }

        public abstract bool Equals(MainLogItemViewModel other);
    }
}
