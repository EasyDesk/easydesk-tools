using EasyDesk.Tools.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static EasyDesk.Tools.Functions;
using static EasyDesk.Tools.Options.OptionImports;

namespace EasyDesk.Tools.Collections
{
    public static class EnumerableUtils
    {
        public static IEnumerable<T> Iterate<T>(T seed, Func<T, T> next)
        {
            var curr = seed;
            while (true)
            {
                yield return curr;
                curr = next(curr);
            }
        }

        public static IEnumerable<T> Generate<T>(Func<T> supplier)
        {
            while (true)
            {
                yield return supplier();
            }
        }

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (var item in sequence)
            {
                action(item);
            }
        }

        public static IEnumerable<T> Peek<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (var item in sequence)
            {
                action(item);
                yield return item;
            }
        }

        public static Option<T> FirstOption<T>(this IEnumerable<T> sequence)
        {
            using (var enumerator = sequence.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return None;
                }
                return Some(enumerator.Current); 
            }
        }

        public static Option<T> FirstOption<T>(this IEnumerable<T> sequence, Func<T, bool> predicate)
        {
            return sequence.Where(predicate).FirstOption();
        }

        public static Option<T> SingleOption<T>(this IEnumerable<T> sequence)
        {
            using (var enumerator = sequence.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return None;
                }
                var itemToReturn = enumerator.Current;
                if (enumerator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains more than one element");
                }
                return Some(itemToReturn);
            }
        }

        public static Option<T> SingleOption<T>(this IEnumerable<T> sequence, Func<T, bool> predicate)
        {
            return sequence.Where(predicate).SingleOption();
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> sequence)
        {
            return sequence.SelectMany(x => x);
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<Option<T>> sequence)
        {
            return sequence.SelectMany(x => x);
        }

        public static string ConcatStrings<T>(this IEnumerable<T> sequence)
        {
            return sequence.ConcatStrings(string.Empty);
        }

        public static string ConcatStrings<T>(this IEnumerable<T> sequence, string separator)
        {
            return sequence.ConcatStrings(separator, string.Empty, string.Empty);
        }

        public static string ConcatStrings<T>(this IEnumerable<T> sequence, string separator, string prefix, string suffix)
        {
            return new StringBuilder()
                .Append(prefix)
                .AppendJoin(separator, sequence)
                .Append(suffix)
                .ToString();
        }

        public static IEnumerable<T> Items<T>(params T[] items) => items;

        public static IEnumerable<TOut> Scan<T, TOut>(this IEnumerable<T> input, TOut seed, Func<TOut, T, TOut> next)
        {
            yield return seed;
            foreach (var item in input)
            {
                seed = next(seed, item);
                yield return seed;
            }
        }

        public static R FoldLeft<T, R>(this IEnumerable<T> sequence, R seed, Func<R, T, R> combiner)
        {
            var current = seed;
            foreach (var item in sequence)
            {
                current = combiner(current, item);
            }
            return current;
        }

        public static R FoldRight<T, R>(this IEnumerable<T> sequence, R seed, Func<T, R, R> combiner) =>
            sequence.Reverse().FoldLeft(seed, (a, b) => combiner(b, a));

        public static bool IsSorted<T>(this IEnumerable<T> sequence) where T : IComparable<T> =>
            sequence.MatchesTwoByTwo((a, b) => a.IsLessThanOrEqualTo(b));

        public static bool IsStrictlySorted<T>(this IEnumerable<T> sequence) where T : IComparable<T> =>
            sequence.MatchesTwoByTwo((a, b) => a.IsLessThan(b));

        public static bool IsSortedDescending<T>(this IEnumerable<T> sequence) where T : IComparable<T> =>
            sequence.MatchesTwoByTwo((a, b) => a.IsGreaterThanOrEqualTo(b));

        public static bool IsStrictlySortedDescending<T>(this IEnumerable<T> sequence) where T : IComparable<T> =>
            sequence.MatchesTwoByTwo((a, b) => a.IsGreaterThan(b));

        public static bool AllSame<T>(this IEnumerable<T> sequence) =>
            sequence.MatchesTwoByTwo((a, b) => a.Equals(b));

        public static bool MatchesTwoByTwo<T>(this IEnumerable<T> sequence, Func<T, T, bool> predicate)
        {
            using (var enumerator = sequence.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return true;
                }
                var previous = enumerator.Current;
                while (enumerator.MoveNext())
                {
                    if (!predicate(previous, enumerator.Current))
                    {
                        return false;
                    }
                    previous = enumerator.Current;
                }
                return true;
            }
        }

        public static IEnumerable<(T Item, TTemp Scan)> ZipScan<T, TTemp>(this IEnumerable<T> input, TTemp seed, Func<TTemp, T, TTemp> next)
        {
            foreach (var item in input)
            {
                seed = next(seed, item);
                yield return (item, seed);
            }
        }

        public static Option<T> MaxOption<T>(this IEnumerable<T> sequence)
            where T : IComparable<T>
        {
            return sequence.MinOrMaxBy(It, GreaterThan);
        }

        public static Option<T> MinOption<T>(this IEnumerable<T> sequence)
            where T : IComparable<T>
        {
            return sequence.MinOrMaxBy(It, LessThan);
        }

        public static Option<T> MaxByOption<T, U>(this IEnumerable<T> sequence, Func<T, U> property)
            where U : IComparable<U>
        {
            return sequence.MinOrMaxBy(property, GreaterThan);
        }

        public static Option<T> MinByOption<T, U>(this IEnumerable<T> sequence, Func<T, U> property)
            where U : IComparable<U>
        {
            return sequence.MinOrMaxBy(property, LessThan);
        }

        public static T MaxBy<T, U>(this IEnumerable<T> sequence, Func<T, U> property)
            where U : IComparable<U>
        {
            return sequence.MaxByOption(property).OrElseThrow(SequenceIsEmptyException);
        }

        public static T MinBy<T, U>(this IEnumerable<T> sequence, Func<T, U> property)
            where U : IComparable<U>
        {
            return sequence.MinByOption(property).OrElseThrow(SequenceIsEmptyException);
        }

        private static Option<T> MinOrMaxBy<T, U>(this IEnumerable<T> sequence, Func<T, U> property, Func<int, bool> direction)
            where U : IComparable<U>
        {
            var empty = true;
            T currentItem = default;
            U currentValue = default;
            foreach (var item in sequence)
            {
                if (empty)
                {
                    empty = false;
                    currentItem = item;
                    currentValue = property(item);
                }
                var value = property(item);
                if (direction(value.CompareTo(currentValue)))
                {
                    currentItem = item;
                    currentValue = value;
                }
            }
            return empty ? None : Some(currentItem);
        }

        private static Exception SequenceIsEmptyException() => new InvalidOperationException("Sequence is empty");
    }
}
