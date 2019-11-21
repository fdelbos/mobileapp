using System;

namespace Toggl.Core.UI.ViewModels.TimeEntriesLog.Identity
{
    internal sealed class UserFeedbackKey : IMainLogKey
    {

        public UserFeedbackKey()
        {
        }

        public bool Equals(IMainLogKey otherKey)
            => otherKey is SuggestionsSectionKey;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is SuggestionsSectionKey other && Equals(other);
        }
    }
}
