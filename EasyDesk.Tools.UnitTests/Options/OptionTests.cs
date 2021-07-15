using NSubstitute;
using Shouldly;
using System;
using Xunit;
using static EasyDesk.Core.Options.OptionImports;

namespace EasyDesk.Core.UnitTests.Options
{
    public class OptionTests
    {
        private const int _value = 5;

        [Fact]
        public void IsPresent_ShouldBeTrue_OnlyIfOptionIsNotEmpty()
        {
            NoneT<int>().IsPresent.ShouldBeFalse();
            Some(_value).IsPresent.ShouldBeTrue();
        }

        [Fact]
        public void IsAbsent_ShouldBeTrue_OnlyIfOptionIsEmpty()
        {
            NoneT<int>().IsAbsent.ShouldBeTrue();
            Some(_value).IsAbsent.ShouldBeFalse();
        }

        [Fact]
        public void ReadingValue_ShouldFail_IfOptionIsEmpty()
        {
            Assert.Throws<InvalidOperationException>(() => NoneT<int>().Value);
        }

        [Fact]
        public void ReadingValue_ShouldSucceed_IfOptionIsNotEmpty()
        {
            Some(_value).Value.ShouldBe(_value);
        }

        [Fact]
        public void MatchWithResult_ShouldReturnTheNoneBranch_IfOptionIsEmpty()
        {
            var shouldNotBeCalled = Substitute.For<Func<int, int>>();

            var res = NoneT<int>().Match(
                some: shouldNotBeCalled,
                none: () => _value);

            res.ShouldBe(_value);
            shouldNotBeCalled.DidNotReceiveWithAnyArgs()(default);
        }

        [Fact]
        public void MatchWithResult_ShouldReturnTheSomeBranch_IfOptionIsNotEmpty()
        {
            var shouldNotBeCalled = Substitute.For<Func<int>>();

            var res = Some(_value).Match(
                some: v => v + 1,
                none: shouldNotBeCalled);

            res.ShouldBe(_value + 1);
            shouldNotBeCalled.DidNotReceiveWithAnyArgs()();
        }

        [Fact]
        public void MatchWithActions_ShouldCallTheNoneBranchOnly_IfOptionIsEmpty()
        {
            var shouldNotBeCalled = Substitute.For<Action<int>>();
            var shouldBeCalled = Substitute.For<Action>();

            NoneT<int>().Match(
                some: shouldNotBeCalled,
                none: shouldBeCalled);

            shouldNotBeCalled.DidNotReceiveWithAnyArgs()(default);
            shouldBeCalled.Received(1)();
        }

        [Fact]
        public void MatchWithActions_ShouldCallTheSomeBranchOnly_IfOptionIsNotEmpty()
        {
            var shouldBeCalled = Substitute.For<Action<int>>();
            var shouldNotBeCalled = Substitute.For<Action>();

            Some(_value).Match(
                some: shouldBeCalled,
                none: shouldNotBeCalled);

            shouldNotBeCalled.DidNotReceiveWithAnyArgs()();
            shouldBeCalled.Received(1)(_value);
        }
    }
}
