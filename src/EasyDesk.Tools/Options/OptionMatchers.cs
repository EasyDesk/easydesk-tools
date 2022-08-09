using System;
using System.Threading.Tasks;

namespace EasyDesk.Tools;

public static partial class StaticImports
{
    public static T OrElseGet<T>(this Option<T> option, Func<T> supplier) =>
        option.Match(some: t => t, none: supplier);

    public static Task<T> OrElseGetAsync<T>(this Option<T> option, AsyncFunc<T> supplier) =>
        option.MatchAsync(some: t => Task.FromResult(t), none: supplier);

    public static T OrElse<T>(this Option<T> option, T defaultValue) =>
        option.OrElseGet(() => defaultValue);

    public static T OrElseDefault<T>(this Option<T> option) =>
        option.OrElse(default);

    public static T OrElseNull<T>(this Option<T> option) where T : class =>
        option.OrElse(null);

    public static T OrElseThrow<T>(this Option<T> option, Func<Exception> exceptionSupplier) =>
        option.OrElseGet(() => throw exceptionSupplier());

    public static T? AsNullable<T>(this Option<T> option) where T : struct =>
        option.Match(some: t => t, none: () => null as T?);
}
