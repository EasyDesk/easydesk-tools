using System;

namespace EasyDesk.Tools.Options
{
    public static partial class OptionImports
    {
        public static NoneOption None => NoneOption.Value;

        public static Option<T> NoneT<T>() => new();

        public static Option<T> Some<T>(T value) =>
            value is null ? throw new ArgumentNullException(nameof(value)) : new(value);

        public static Option<T> AsOption<T>(this T value) where T : class =>
            value is null ? None : Some(value);

        public static Option<T> AsOption<T>(this T? value) where T : struct =>
            value is null ? None : Some(value.Value);
    }
}
