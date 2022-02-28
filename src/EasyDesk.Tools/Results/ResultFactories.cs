using System;

namespace EasyDesk.Tools.Results;

public static partial class ResultImports
{
    public static Result<Nothing> Ok { get; } = Success(Nothing.Value);

    public static Result<T> Success<T>(T data) => new(data);

    public static Result<T> Failure<T>(Error error) => new(error);

    public static Result<Nothing> Ensure(bool condition, Func<Error> otherwise) =>
        condition ? Ok : Failure<Nothing>(otherwise());

    public static Result<Nothing> EnsureNot(bool condition, Func<Error> otherwise) =>
        Ensure(!condition, otherwise);
}
