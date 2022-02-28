using System;

namespace EasyDesk.Tools.Results;

public static partial class ResultImports
{
    public static Result<R> Select<T, R>(this Result<T> result, Func<T, R> mapper) =>
        result.Map(mapper);

    public static Result<R> SelectMany<T, R>(this Result<T> result, Func<T, Result<R>> mapper) =>
        result.FlatMap(mapper);

    public static Result<R> SelectMany<T, X, R>(this Result<T> result, Func<T, Result<X>> mapper, Func<T, X, R> project) =>
        result.FlatMap(x => mapper(x).Map(y => project(x, y)));
}
