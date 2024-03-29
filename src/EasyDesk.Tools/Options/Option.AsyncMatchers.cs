﻿namespace EasyDesk.Tools;

public static partial class StaticImports
{
    public static async Task<R> ThenMatch<T, R>(this Task<Option<T>> option, Func<T, R> some, Func<R> none) =>
        (await option).Match(some, none);

    public static async Task<R> ThenMatchAsync<T, R>(this Task<Option<T>> option, AsyncFunc<T, R> some, AsyncFunc<R> none) =>
        await (await option).MatchAsync(some, none);

    public static async Task ThenMatch<T>(this Task<Option<T>> option, Action<T> some, Action none) =>
        (await option).Match(some, none);

    public static async Task ThenMatchAsync<T>(this Task<Option<T>> option, AsyncAction<T> some, AsyncAction none) =>
        await (await option).MatchAsync(some, none);
}
