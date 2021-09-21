using System.Threading.Tasks;

namespace EasyDesk.Tools
{
    public struct Nothing
    {
        public static Nothing Value { get; } = default;

        public static Task<Nothing> ValueAsync { get; } = Task.FromResult(Value);
    }
}
