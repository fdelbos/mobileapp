using System;
using Android.Support.V7.Widget;
using Toggl.Core.UI.Interfaces;
using Toggl.Shared;

namespace Toggl.Droid.Adapters.DiffingStrategies
{
    public sealed class TypeIdentifierEqualityDiffingStrategy<T> : IDiffingStrategy<T>
        where T : IEquatable<T>
    {
        public TypeIdentifierEqualityDiffingStrategy()
        {
            Ensure.Argument.TypeImplementsOrInheritsFromType(
                derivedType: typeof(T),
                baseType: typeof(IDiffableByIdentifier<T>));
        }

        public bool AreContentsTheSame(T item, T other)
        {
            return item.Equals(other);
        }

        public bool AreItemsTheSame(T item, T other)
        {
            if (item.GetType() != other.GetType())
            {
                return false;
            }

            var itemDiffable = (IDiffableByIdentifier<T>)item;
            var otherDiffable = (IDiffableByIdentifier<T>)item;

            return itemDiffable.Identifier == otherDiffable.Identifier;
        }

        public long GetItemId(T item) => RecyclerView.NoId;

        public bool HasStableIds => false;
    }
}
