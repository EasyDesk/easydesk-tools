﻿namespace EasyDesk.Tools.Utils;

public readonly record struct Nothing
{
    public static Nothing Value { get; } = default;

    public static Task<Nothing> ValueAsync { get; } = Task.FromResult(Value);
}
