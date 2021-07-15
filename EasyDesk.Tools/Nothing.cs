using System.Threading.Tasks;

namespace EasyDesk.Core
{
    public struct Nothing
    {
        public static Nothing Value { get; } = new();

        public static Task<Nothing> ValueAsync { get; } = Task.FromResult(Value);
    }
}
