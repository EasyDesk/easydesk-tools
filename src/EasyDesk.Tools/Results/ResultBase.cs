using EasyDesk.Tools.Options;
using System;
using System.Threading.Tasks;
using static EasyDesk.Tools.Functions;
using static EasyDesk.Tools.Options.OptionImports;

namespace EasyDesk.Tools.Results;

public abstract record ResultBase<T, E>
{
    private readonly T _value;
    private readonly E _error;

    protected ResultBase(T value)
    {
        _value = value;
        IsFailure = false;
    }

    protected ResultBase(E error)
    {
        _error = error;
        IsFailure = true;
    }

    public bool IsSuccess => !IsFailure;

    public bool IsFailure { get; }

    public Option<T> Value => Match(
        success: t => Some(t),
        failure: _ => None);

    public Option<E> Error => Match(
        success: _ => None,
        failure: e => Some(e));

    public T ReadValue() => Match(
        success: t => t,
        failure: _ => throw new InvalidOperationException("Cannot read value from an error result"));

    public E ReadError() => Match(
        success: _ => throw new InvalidOperationException("Cannot read error from a successful result"),
        failure: e => e);

    public R Match<R>(Func<T, R> success, Func<E, R> failure) => IsFailure ? failure(_error) : success(_value);

    public void Match(Action<T> success = null, Action<E> failure = null)
    {
        Match(
            success: t => Execute(() => success?.Invoke(t)),
            failure: e => Execute(() => failure?.Invoke(e)));
    }

    public Task<R> MatchAsync<R>(AsyncFunc<T, R> success, AsyncFunc<E, R> failure) =>
        Match(
            success: a => success(a),
            failure: e => failure(e));

    public Task MatchAsync(AsyncAction<T> success = null, AsyncAction<E> failure = null) =>
        Match(
            success: a => success is null ? Task.CompletedTask : success(a),
            failure: e => failure is null ? Task.CompletedTask : failure(e));
}
