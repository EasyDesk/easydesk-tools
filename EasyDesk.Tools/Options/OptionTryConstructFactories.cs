namespace EasyDesk.Tools.Options
{
    public static partial class OptionImports
    {
        public delegate bool TryConstruct<T>(out T output);

        public delegate bool TryConstruct<T1, T>(T1 arg1, out T output);

        public static Option<T> FromTryConstruct<T1, T>(T1 arg1, TryConstruct<T1, T> tryConstruct) =>
            tryConstruct(arg1, out var output) ? Some(output) : None;

        public static Option<T> FromTryConstruct<T>(TryConstruct<T> tryConstruct) =>
            tryConstruct(out var output) ? Some(output) : None;
    }
}
