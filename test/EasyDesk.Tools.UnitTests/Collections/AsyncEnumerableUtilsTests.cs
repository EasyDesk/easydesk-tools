using EasyDesk.Tools.Collections;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static System.Linq.Enumerable;

namespace EasyDesk.Tools.UnitTests.Collections;

public class AsyncEnumerableUtilsTests
{
    private async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> sequence)
    {
        await Task.CompletedTask;
        foreach (var item in sequence)
        {
            yield return item;
        }
    }

    [Fact]
    public async void ForEach_ShouldCallTheGivenAsyncActionForEveryElementInTheSquence()
    {
        var count = 10;
        var action = Substitute.For<Action<int>>();
        var range = ToAsyncEnumerable(Range(0, count));

        await range.ForEach(action);

        await action.ReceivedWithAnyArgs(10)(default);
        Received.InOrder(async () =>
        {
            await foreach (var i in range)
            {
                await action(i);
            }
        });
    }

    [Fact]
    public async void ForEach_ShouldCallTheGivenActionForEveryElementInTheSquence()
    {
        var count = 10;
        var action = Substitute.For<Action<int>>();
        var range = ToAsyncEnumerable(Range(0, count));

        await range.ForEach(action);

        await action.ReceivedWithAnyArgs(10)(default);
        Received.InOrder(async () =>
        {
            await foreach (var i in range)
            {
                await action(i);
            }
        });
    }
}
