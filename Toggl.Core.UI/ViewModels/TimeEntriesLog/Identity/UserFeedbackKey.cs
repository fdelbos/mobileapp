using System;

namespace Toggl.Core.UI.ViewModels.TimeEntriesLog.Identity
{
    internal sealed class UserFeedbackKey : IMainLogKey
    {
        public long Identifier()
            => GetHashCode();

        public bool Equals(IMainLogKey otherKey)
            => otherKey is SuggestionsSectionKey;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is SuggestionsSectionKey other && Equals(other);
        }

        public override int GetHashCode()
            => 11235813;
    }
}
