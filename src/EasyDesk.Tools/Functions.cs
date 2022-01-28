using System;
using System.Threading.Tasks;

namespace EasyDesk.Tools;

public static class Functions
{
    public static T It<T>(T t) => t;

    public static bool LessThan(int compareResult) => compareResult < 0;

    public static bool LessThanOrEqualTo(int compareResult) => compareResult <= 0;

    public static bool GreaterThan(int compareResult) => compareResult > 0;

    public static bool GreaterThanOrEqualTo(int compareresult) => compareresult >= 0;

    public static Nothing Execute(Action action)
    {
        action();
        return Nothing.Value;
    }

    public static async Task<Nothing> Execute(AsyncAction action)
    {
        await action();
        return Nothing.Value;
    }
}
