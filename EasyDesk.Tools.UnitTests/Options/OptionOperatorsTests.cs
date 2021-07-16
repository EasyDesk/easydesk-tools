using EasyDesk.Tools.Options;
using NSubstitute;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;
using static EasyDesk.Tools.Options.OptionImports;

namespace EasyDesk.Tools.UnitTests.Options
{
    public class OptionOperatorsTests
    {
        private const int _value = 5;
        private const int _other = 10;

        [Fact]
        public void IfPresent_ShouldNotCallTheGivenAction_IfOptionIsEmpty()
        {
            var action = Substitute.For<Action<int>>();

            NoneT<int>().IfPresent(action);

            action.DidNotReceiveWithAnyArgs()(default);
        }

        [Fact]
        public void IfPresent_ShouldCallTheGivenAction_IfOptionIsNotEmpty()
        {
            var action = Substitute.For<Action<int>>();

            Some(_value).IfPresent(action);

            action.Received(1)(_value);
        }

        [Fact]
        public void IfAbsent_ShouldCallTheGivenAction_IfOptionIsEmpty()
        {
            var action = Substitute.For<Action>();

            NoneT<int>().IfAbsent(action);

            action.Received(1)();
        }

        [Fact]
        public void IfAbsent_ShouldNotCallTheGivenAction_IfOptionIsNotEmpty()
        {
            var action = Substitute.For<Action>();

            Some(_value).IfAbsent(action);

            action.DidNotReceiveWithAnyArgs()();
        }

        [Fact]
        public void Map_ShouldReturnNone_IfOptionIsEmpty()
        {
            NoneT<int>().Map(x => x + 1).ShouldBe(None);
        }

        [Fact]
        public void Map_ShouldReturnTheMappedValue_IfOptionIsNotEmpty()
        {
            Some(_value).Map(x => x + 1).ShouldBe(Some(_value + 1));
        }

        [Fact]
        public void Filter_ShouldReturnNone_IfOptionIsEmpty()
        {
            NoneT<int>().Filter(_ => true).ShouldBe(None);
        }

        [Fact]
        public void Filter_ShouldReturnNone_IfOptionIsNotEmptyButPredicateIsNotMatched()
        {
            Some(_value).Filter(x => x != _value).ShouldBe(None);
        }

        [Fact]
        public void Filter_ShouldReturnTheSameOption_IfOptionIsNotEmptyAndPredicateIsMatched()
        {
            var some = Some(_value);
            some.Filter(x => x == _value).ShouldBe(some);
        }

        [Fact]
        public void FlatMap_ShouldReturnNone_IfOptionIsEmpty()
        {
            NoneT<int>().FlatMap(_ => Some(_value)).ShouldBe(None);
        }

        [Fact]
        public void FlatMap_ShouldReturnTheResultOfTheMapper_IfOptionIsNotEmpty()
        {
            Some(_value).FlatMap(_ => NoneT<int>()).ShouldBe(None);
            Some(_value).FlatMap(v => Some(v + 1)).ShouldBe(Some(_value + 1));
        }

        [Fact]
        public void Flatten_ShouldReturnNone_IfAnyOptionIsNone()
        {
            Some(NoneT<int>()).Flatten().ShouldBe(None);
            NoneT<Option<int>>().Flatten().ShouldBe(None);
        }

        [Fact]
        public void Flatten_ShouldReturnTheInnermostValue_IfBothOptionsAreNotEmpty()
        {
            Some(Some(_value)).Flatten().ShouldBe(Some(_value));
        }

        [Fact]
        public void Or_ShouldShortCircuit_IfTheFirstOptionIsNotEmpty()
        {
            var first = Some(_value);
            first.Or(NoneT<int>()).ShouldBe(first);
            first.Or(Some(_other)).ShouldBe(first);
        }

        [Fact]
        public void Or_ShouldReturnTheSecondOption_IfTheFirstIsEmpty()
        {
            NoneT<int>().Or(Some(_value)).ShouldBe(Some(_value));
            NoneT<int>().Or(NoneT<int>()).ShouldBe(None);
        }

        [Fact]
        public async Task IfPresentAsync_ShouldNotCallTheGivenAsyncAction_IfOptionIsEmpty()
        {
            var action = Substitute.For<AsyncAction<int>>();

            await NoneT<int>().IfPresentAsync(action);

            await action.DidNotReceiveWithAnyArgs()(default);
        }

        [Fact]
        public async Task IfPresentAsync_ShouldCallTheGivenAsyncAction_IfOptionIsNotEmpty()
        {
            var action = Substitute.For<AsyncAction<int>>();

            await Some(_value).IfPresentAsync(action);

            await action.Received(1)(_value);
        }

        [Fact]
        public async Task IfAbsentAsync_ShouldCallTheGivenAsyncAction_IfOptionIsEmpty()
        {
            var action = Substitute.For<AsyncAction>();

            await NoneT<int>().IfAbsentAsync(action);

            await action.Received(1)();
        }

        [Fact]
        public async Task IfAbsentAsync_ShouldNotCallTheGivenAsyncAction_IfOptionIsNotEmpty()
        {
            var action = Substitute.For<AsyncAction>();

            await Some(_value).IfAbsentAsync(action);

            await action.DidNotReceiveWithAnyArgs()();
        }

        [Fact]
        public async Task MapAsync_ShouldReturnNone_IfOptionIsEmpty()
        {
            var result = await NoneT<int>().MapAsync(x => Task.FromResult(x + 1));

            result.ShouldBe(None);
        }

        [Fact]
        public async Task MapAsync_ShouldReturnTheMappedValue_IfOptionIsNotEmptyAsync()
        {
            var result = await Some(_value).MapAsync(x => Task.FromResult(x + 1));

            result.ShouldBe(Some(_value + 1));
        }

        [Fact]
        public async Task FilterAsync_ShouldReturnNone_IfOptionIsEmpty()
        {
            var result = await NoneT<int>().FilterAsync(_ => Task.FromResult(true));

            result.ShouldBe(None);
        }

        [Fact]
        public async Task FilterAsync_ShouldReturnNone_IfOptionIsNotEmptyButPredicateIsNotMatched()
        {
            var result = await Some(_value).FilterAsync(x => Task.FromResult(x != _value));

            result.ShouldBe(None);
        }

        [Fact]
        public async Task FilterAsync_ShouldReturnTheSameOption_IfOptionIsNotEmptyAndPredicateIsMatched()
        {
            var some = Some(_value);

            var result = await some.FilterAsync(x => Task.FromResult(x == _value));

            result.ShouldBe(some);
        }

        [Fact]
        public async Task FlatMapAsync_ShouldReturnNone_IfOptionIsEmpty()
        {
            var result = await NoneT<int>().FlatMapAsync(_ => Task.FromResult(Some(_value)));

            result.ShouldBe(None);
        }

        [Fact]
        public async Task FlatMapAsync_ShouldReturnNone_IfOptionIsNotEmptyAndMapperReturnsNone()
        {
            var result = await Some(_value).FlatMapAsync(_ => Task.FromResult(NoneT<int>()));

            result.ShouldBe(None);
        }

        [Fact]
        public async Task FlatMapAsync_ShouldReturnTheValueReturnedByTheGivenFunction_IfOptionIsNotEmpty()
        {
            var result = await Some(_value).FlatMapAsync(v => Task.FromResult(Some(v + 1)));

            result.ShouldBe(Some(_value + 1));
        }
    }
}
