using EasyDesk.Tools.Utils;
using System.Threading.Tasks;

namespace EasyDesk.Tools.Observables;

public interface IAsyncEmitter<in T>
{
    Task Emit(T value);
}

public static class AsyncEmitterExtensions
{
    public static Task Emit(this IAsyncEmitter<Nothing> emitter) => emitter.Emit(Nothing.Value);
}
