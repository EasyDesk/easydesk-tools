using EasyDesk.Core.Options;
using System;
using System.Collections.Generic;
using static EasyDesk.Core.Options.OptionImports;

namespace EasyDesk.Core.Collections
{
    public static class DictionaryUtils
    {
        public static Option<V> GetOption<K, V>(this IDictionary<K, V> dictionary, K key) =>
            FromTryConstruct<K, V>(key, dictionary.TryGetValue);

        public static void Merge<K, V>(this IDictionary<K, V> dictionary, K key, V value, Func<V, V, V> combiner)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = combiner(dictionary[key], value);
            }
            else
            {
                dictionary.Add(key, value);
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
}
