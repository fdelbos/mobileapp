using System;
using Android.Support.V7.Widget;
using Toggl.Core.UI.Collections;
using Toggl.Core.UI.Interfaces;
using Toggl.Shared;

namespace Toggl.Droid.Adapters.DiffingStrategies
{
    public sealed class TypeIdentifierEqualityDiffingStrategy<T, KT> : IDiffingStrategy<T>
        where T : IEquatable<T>
        where KT : IEquatable<KT>
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
            var itemDiffable = (IDiffable<KT>) item;
            var otherDiffable = (IDiffable<KT>) other;

            if (itemDiffable.Identity.GetType() != otherDiffable.Identity.GetType())
            {
                return false;
            }

            var itemDiffableById = (IDiffableByIdentifier<T>)item;
            var otherDiffableById = (IDiffableByIdentifier<T>)item;

            return itemDiffableById.Identifier == otherDiffableById.Identifier;
        }

        public long GetItemId(T item) => RecyclerView.NoId;

        public bool HasStableIds => false;
    }
}
