using EasyDesk.Tools.Collections.Immutable;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace EasyDesk.Tools.Collections
{
    public static class ImmutableCollections
    {
        public delegate IImmutableSet<T> SetMutation<T>(IImmutableSet<T> set);

        public delegate IImmutableList<T> ListMutation<T>(IImmutableList<T> list);

        public delegate IImmutableDictionary<K, V> DictionaryMutation<K, V>(IImmutableDictionary<K, V> dictionary);

        public static IImmutableSet<T> Set<T>(params T[] items) => Set(items as IEnumerable<T>);

        public static IImmutableSet<T> Set<T>(IEnumerable<T> items) => Set(items, EqualityComparer<T>.Default);

        public static IImmutableSet<T> Set<T>(IEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            var set = ImmutableHashSet.CreateRange(comparer, items);
            return EquatableImmutableSet<T>.FromHashSet(set);
        }

        public static IImmutableList<T> List<T>(params T[] items) => List(items as IEnumerable<T>);

        public static IImmutableList<T> List<T>(IEnumerable<T> items) => ImmutableList.CreateRange(items);

        public static IImmutableDictionary<K, V> Map<K, V>(params (K Key, V Value)[] items) =>
            Map(items as IEnumerable<(K, V)>);

        public static IImmutableDictionary<K, V> Map<K, V>(IEnumerable<(K Key, V Value)> items)
        {
            var pairs = items.Select(x => new KeyValuePair<K, V>(x.Key, x.Value));
            return Map(pairs);
        }

        public static IImmutableDictionary<K, V> Map<K, V>(IEnumerable<KeyValuePair<K, V>> pairs)
        {
            var dictionary = ImmutableDictionary.CreateRange(pairs);
            return EquatableImmutableDictionary<K, V>.FromDictionary(dictionary);
        }
    }
}
