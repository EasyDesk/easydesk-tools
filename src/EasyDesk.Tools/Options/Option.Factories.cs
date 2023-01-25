namespace EasyDesk.Tools;

public static partial class StaticImports
{
    public static NoneOption None => NoneOption.Value;

    public static Option<T> NoneT<T>() => default;

    public static Option<T> Some<T>(T value) =>
        value is null ? throw new ArgumentNullException(nameof(value)) : new(value);

    public static Option<T> AsSome<T>(this T value) where T : notnull => Some(value);

    public static Option<T> AsOption<T>(this T? value) where T : class =>
        value is null ? None : Some(value);

    public static Option<T> AsOption<T>(this T? value) where T : struct =>
        value is null ? None : Some(value ?? throw new InvalidOperationException());

    public static Option<T> ToOption<T>(this T? value) =>
        value is null ? None : Some(value);
}
