using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyDesk.Tools.Collections;

public static class AsyncEnumerableUtils
{
    public static async Task ForEach<T>(this IAsyncEnumerable<T> sequence, Action<T> action)
    {
        await foreach (var item in sequence)
        {
            await action(item);
        }
    }

    public static async Task ForEach<T>(this IAsyncEnumerable<T> sequence, System.Action<T> action)
    {
        await foreach (var item in sequence)
        {
            action(item);
        }
    }
}
