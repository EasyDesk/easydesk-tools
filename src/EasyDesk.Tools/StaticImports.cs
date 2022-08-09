using EasyDesk.Tools.Utils;
using System;
using System.Threading.Tasks;

namespace EasyDesk.Tools;

public static partial class StaticImports
{
    public static T It<T>(T t) => t;

    public static Nothing ReturningNothing(Action action)
    {
        action();
        return Nothing.Value;
    }

    public static async Task<Nothing> ReturningNothing(AsyncAction action)
    {
        await action();
        return Nothing.Value;
    }
}
