using EasyDesk.Tools.Options;
using System;
using System.Collections.Generic;
using static EasyDesk.Tools.Options.OptionImports;

namespace EasyDesk.Tools.Collections;

public static class DictionaryUtils
{
    public static Option<V> GetOption<K, V>(this IDictionary<K, V> dictionary, K key) =>
        FromTryConstruct<K, V>(key, dictionary.TryGetValue);

    public static bool Merge<K, V>(this IDictionary<K, V> dictionary, K key, V value, Func<V, V, V> combiner)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = combiner(dictionary[key], value);
            return false;
        }
        else
        {
            dictionary.Add(key, value);
            return true;
        }
    }

    public static V GetOrAdd<K, V>(this IDictionary<K, V> dictionary, K key, Func<V> supplier)
    {
        if (dictionary.ContainsKey(key))
        {
            return dictionary[key];
        }
        else
        {
            var value = supplier();
            dictionary.Add(key, value);
            return value;
        }
    }
}
