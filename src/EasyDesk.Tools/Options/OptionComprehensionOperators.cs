using System;

namespace EasyDesk.Tools.Options;

public static partial class OptionImports
{
    public static Option<R> Select<T, R>(this Option<T> option, Func<T, R> mapper) =>
        option.Map(mapper);

    public static Option<R> SelectMany<T, R>(this Option<T> option, Func<T, Option<R>> mapper) =>
        option.FlatMap(mapper);

    public static Option<R> SelectMany<T, X, R>(this Option<T> option, Func<T, Option<X>> mapper, Func<T, X, R> project) =>
        option.FlatMap(x => mapper(x).Map(y => project(x, y)));

    public static Option<T> Where<T>(this Option<T> option, Predicate<T> predicate) =>
        option.Filter(predicate);
}
