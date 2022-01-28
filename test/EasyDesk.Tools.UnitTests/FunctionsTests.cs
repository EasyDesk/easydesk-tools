using NSubstitute;
using System;
using Xunit;

namespace EasyDesk.Tools.UnitTests;

public class FunctionsTests
{
    [Fact]
    public void Execute_ShouldInvokeAction()
    {
        var action = Substitute.For<Action>();
        Functions.Execute(action);
        action.Received(1)();
    }

    [Fact]
    public async void Execute_ShouldInvokeAsyncAction()
    {
        var action = Substitute.For<AsyncAction>();
        await Functions.Execute(action);
        await action.Received(1)();
    }
}
