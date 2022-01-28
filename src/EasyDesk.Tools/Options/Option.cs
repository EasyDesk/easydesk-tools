﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EasyDesk.Tools.Options.OptionImports;

namespace EasyDesk.Tools.Options;

public readonly struct NoneOption
{
    public static NoneOption Value { get; } = default;
}

public readonly struct Option<T> : IEnumerable<T>
{
    private readonly T _value;

    internal Option(T value)
    {
        IsPresent = true;
        _value = value;
    }

    public T Value => IsPresent ? _value : throw new InvalidOperationException("Cannot access the value of an empty Option");

    public bool IsPresent { get; }

    public bool IsAbsent => !IsPresent;

    public IEnumerator<T> GetEnumerator()
    {
        if (IsPresent)
        {
            yield return Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public R Match<R>(Func<T, R> some, Func<R> none) =>
       IsPresent ? some(_value) : none();

    public Task<R> MatchAsync<R>(AsyncFunc<T, R> some, AsyncFunc<R> none) =>
       Match(some: t => some(t), none: () => none());

    public void Match(System.Action<T> some, Action none)
    {
        if (IsPresent)
        {
            some(_value);
        }
        else
        {
            none();
        }
    }

    public async Task MatchAsync(Action<T> some, AsyncAction none)
    {
        if (IsPresent)
        {
            await some(_value);
        }
        else
        {
            await none();
        }
    }

#pragma warning disable IDE0060
    public static implicit operator Option<T>(NoneOption none) => default;
#pragma warning restore IDE0060

    public static implicit operator Option<T>(T value) => Some(value);

    public static T operator |(Option<T> a, T b) => a.OrElse(b);

    public static T operator |(Option<T> a, Func<T> b) => a.OrElseGet(b);

    public static Option<T> operator |(Option<T> a, Option<T> b) => a.Match(some: _ => a, none: () => b);

    public static Option<T> operator &(Option<T> a, Option<T> b) => a.Match(some: _ => b, none: () => a);

    public static Option<T> operator ^(Option<T> a, Option<T> b) => a.Match(
        some: _ => b.Match(some: _ => None, none: () => a),
        none: () => b);

    public static bool operator true(Option<T> a) => a.IsPresent;

    public static bool operator false(Option<T> a) => a.IsAbsent;

    public override string ToString() => Match(
        some: t => $"Some({t})",
        none: () => "None");
}
