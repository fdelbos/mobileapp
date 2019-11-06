namespace Toggl.Core.UI.ViewModels.TimeEntriesLog.Identity
{
    public class SuggestionsSectionKey : IMainLogKey
    {
        public SuggestionsSectionKey()
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
