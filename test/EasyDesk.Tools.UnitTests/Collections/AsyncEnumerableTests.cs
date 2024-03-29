﻿using EasyDesk.Tools.Collections;
using NSubstitute;
using Shouldly;
using Xunit;
using static EasyDesk.Tools.Collections.AsyncEnumerable;

namespace EasyDesk.Tools.UnitTests.Collections;

public class AsyncEnumerableTests
{
    [Fact]
    public async void ForEach_ShouldCallTheGivenAsyncActionForEveryElementInTheSquence()
    {
        var action = Substitute.For<AsyncAction<int>>();

        await Of(1, 2, 3).ForEach(action);

        await action.ReceivedWithAnyArgs(3)(default);
        Received.InOrder(async () =>
        {
            await action(1);
            await action(2);
            await action(3);
        });
    }

    [Fact]
    public async void ForEach_ShouldCallTheGivenActionForEveryElementInTheSquence()
    {
        var action = Substitute.For<Action<int>>();

        await Of(1, 2, 3).ForEach(action);

        action.ReceivedWithAnyArgs(3)(default);
        Received.InOrder(() =>
        {
            action(1);
            action(2);
            action(3);
        });
    }

    [Theory]
    [MemberData(nameof(ConcatData))]
    public async Task ThenConcat_ShouldJoinItsArguments(
        IAsyncEnumerable<int> left, IAsyncEnumerable<int> right, IAsyncEnumerable<int> expected)
    {
        (await left.ThenConcat(() => right).SequenceEqualAsync(expected)).ShouldBe(true);
    }

    [Fact]
    public async Task ThenConcat_ShouldNotEvaluateTheSecondArgument_IfNotNecessary()
    {
        var left = Of(1, 2, 3);
        var right = Substitute.For<Func<IAsyncEnumerable<int>>>();

        var enumerator = left.ThenConcat(right).GetAsyncEnumerator();
        await enumerator.MoveNextAsync();
        await enumerator.MoveNextAsync();
        await enumerator.MoveNextAsync();

        right.DidNotReceive()();
    }

    [Fact]
    public async Task ThenConcat_ShouldEvaluateTheSecondArgument_OnlyWhenNecessary()
    {
        var left = Of(1, 2, 3);
        var right = Substitute.For<Func<IAsyncEnumerable<int>>>();
        right().Returns(Of(4, 5));

        var enumerator = left.ThenConcat(right).GetAsyncEnumerator();
        await enumerator.MoveNextAsync();
        await enumerator.MoveNextAsync();
        await enumerator.MoveNextAsync();
        await enumerator.MoveNextAsync();

        right.Received(1)();
    }

    [Theory]
    [MemberData(nameof(ConcatData))]
    public async Task Concat_ShouldJoinItsArguments(
        IAsyncEnumerable<int> left, IAsyncEnumerable<int> right, IAsyncEnumerable<int> expected)
    {
        (await left.Concat(right).SequenceEqualAsync(expected)).ShouldBe(true);
    }

    public static IEnumerable<object[]> ConcatData()
    {
        yield return new object[] { Empty<int>(), Empty<int>(), Empty<int>() };
        yield return new object[] { Empty<int>(), Of(1, 2, 3), Of(1, 2, 3) };
        yield return new object[] { Of(1, 2, 3), Empty<int>(), Of(1, 2, 3) };
        yield return new object[] { Of(1, 2, 3), Of(4, 5, 6), Of(1, 2, 3, 4, 5, 6) };
    }

    [Fact]
    public async Task SequenceEqualAsync_ShouldReturnTrue_IfTheSequencesAreEqualElementByElement()
    {
        (await Of(1, 2, 3).SequenceEqualAsync(Of(1, 2, 3))).ShouldBe(true);
    }

    [Fact]
    public async Task SequenceEqualAsync_ShouldReturnFalse_IfTheFirstSequenceIsShorterThanTheOther()
    {
        (await Of(1, 2, 3).SequenceEqualAsync(Of(1, 2, 3, 4))).ShouldBe(false);
    }

    [Fact]
    public async Task SequenceEqualAsync_ShouldReturnFalse_IfTheFirstSequenceIsLongerThanTheOther()
    {
        (await Of(1, 2, 3, 4).SequenceEqualAsync(Of(1, 2, 3))).ShouldBe(false);
    }

    [Fact]
    public async Task SequenceEqualAsync_ShouldReturnFalse_IfTheSequencesDifferByAtLeastOneElement()
    {
        (await Of(1, 2, 4).SequenceEqualAsync(Of(1, 2, 3))).ShouldBe(false);
    }

    [Fact]
    public async Task SequenceEqualAsync_ShouldUseTheGivenEqualityComparer()
    {
        var comparer = EqualityComparers.FromProperties<string>(x => x.ToLower());
        (await Of("a", "b").SequenceEqualAsync(Of("A", "B"), comparer)).ShouldBe(true);
    }

    [Fact]
    public async Task Select_ShouldMapEveryItem()
    {
        var asyncEnumerable = Of(1, 2, 3);
        var expected = Of("1", "2", "3");
        (await asyncEnumerable.Select(x => x.ToString()).SequenceEqualAsync(expected)).ShouldBe(true);
    }

    [Fact]
    public async Task Where_ShouldFilterItems()
    {
        var asyncEnumerable = Of(1, 2, 3, 4, 5, 6);
        var expected = Of(2, 4, 6);
        (await asyncEnumerable.Where(x => x % 2 == 0).SequenceEqualAsync(expected)).ShouldBe(true);
    }

    [Fact]
    public async Task SelectMany_ShouldMapEveryItemToAnAsyncEnumerable_ThenFlattenTheResults()
    {
        var asyncEnumerable = Of(1, 2, 3);
        var expected = Of("1", "11", "2", "22", "3", "33");
        (await asyncEnumerable.SelectMany(x => Of($"{x}", $"{x}{x}")).SequenceEqualAsync(expected)).ShouldBe(true);
    }

    [Fact]
    public async Task AggregateAsync_ShouldReturnTheSeed_IfTheAsyncEnumerableIsEmpty()
    {
        (await Empty<int>().AggregateAsync("*", (x, y) => x + y)).ShouldBe("*");
    }

    [Fact]
    public async Task AggregateAsync_ShouldAccumulateTheItemsUsingTheGivenFunction()
    {
        (await Of(1, 2, 3).AggregateAsync("*", (x, y) => x + y)).ShouldBe("*123");
    }
}
