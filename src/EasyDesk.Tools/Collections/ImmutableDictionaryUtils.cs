using System.Collections.Immutable;

namespace EasyDesk.Tools.Collections;

public static class ImmutableDictionaryUtils
{
    public static Option<V> GetOption<K, V>(this IImmutableDictionary<K, V> dictionary, K key) =>
        TryOption<K, V>(dictionary.TryGetValue, key);

    public static IImmutableDictionary<K, V> Merge<K, V>(
        this IImmutableDictionary<K, V> dictionary,
        K key,
        V value,
        Func<V, V, V> combiner)
    {
        return dictionary.Update(key, v => combiner(v, value), () => value);
    }

    public static IImmutableDictionary<K, V> Update<K, V>(
        this IImmutableDictionary<K, V> dictionary,
        K key,
        Func<V, V> mutation,
        Func<V> supplier = default)
    {
        return dictionary.GetOption(key).Match(
            some: v => dictionary.SetItem(key, mutation(v)),
            none: () => supplier is null ? dictionary : dictionary.Add(key, supplier()));
    }
}
