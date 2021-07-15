using EasyDesk.Core.Options;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static EasyDesk.Core.Collections.EnumerableUtils;
using static EasyDesk.Core.Options.OptionImports;
using static System.Linq.Enumerable;

namespace EasyDesk.Core.UnitTests.Collections
{
    public class EnumerableUtilsTests
    {
        [Fact]
        public void Iterate_ShouldStartFromSeed()
        {
            Iterate(0, x => x + 1).First().ShouldBe(0);
        }

        [Fact]
        public void Iterate_ShouldUseTheGivenFunctionToComputeTheSequence()
        {
            var count = 10;
            Iterate(0, x => x + 1).Take(count).ShouldBe(Range(0, count));
        }

        [Fact]
        public void Generate_ShouldUseTheGivenSupplierEachTime()
        {
            var count = 10;
            var current = 0;
            Generate(() => current++).Take(count).ShouldBe(Range(0, count));
        }

        [Fact]
        public void ForEach_ShouldCallTheGivenActionForEveryElementInTheSquence()
        {
            var count = 10;
            var action = Substitute.For<Action<int>>();
            var range = Range(0, count);

            range.ForEach(action);

            action.ReceivedWithAnyArgs(10)(default);
            Received.InOrder(() =>
            {
                foreach (var i in range)
                {
                    action(i);
                }
            });
        }

        [Theory]
        [MemberData(nameof(FirstOptionEmptyData))]
        public void FirstOption_ShouldReturnNone_IfNoItemsMatchThePredicate(
            IEnumerable<int> sequence, Func<int, bool> predicate)
        {
            sequence.FirstOption(predicate).ShouldBe(None);
        }

        public static IEnumerable<object[]> FirstOptionEmptyData()
        {
            yield return new object[] { Empty<int>(), new Func<int, bool>(_ => true) };
            yield return new object[] { Empty<int>(), new Func<int, bool>(_ => false) };
            yield return new object[] { Range(5, 10), new Func<int, bool>(x => x > 20) };
        }

        [Theory]
        [MemberData(nameof(FirstOptionNonEmptyData))]
        public void FirstOption_ShouldReturnTheFirstItemMatchingThePredicate_IfAny(
            IEnumerable<int> sequence, Func<int, bool> predicate, int expected)
        {
            sequence.FirstOption(predicate).ShouldBe(Some(expected));
        }

        public static IEnumerable<object[]> FirstOptionNonEmptyData()
        {
            yield return new object[] { Range(5, 10), new Func<int, bool>(x => x > 3), 5 };
            yield return new object[] { Range(5, 10), new Func<int, bool>(x => x > 7), 8 };
        }

        [Theory]
        [MemberData(nameof(SingleOptionWithOneMatchData))]
        public void SingleOption_ShouldReturnTheOnlyItemMatchingThePredicate_IfNoOtherItemsMatchThePredicate(
            IEnumerable<int> sequence, Func<int, bool> predicate, int expected)
        {
            sequence.SingleOption(predicate).ShouldBe(Some(expected));
        }

        public static IEnumerable<object[]> SingleOptionWithOneMatchData()
        {
            yield return new object[] { Range(5, 1), new Func<int, bool>(_ => true), 5 };
            yield return new object[] { Range(5, 10), new Func<int, bool>(x => x is > 7 and < 9), 8 };
        }

        [Theory]
        [MemberData(nameof(SingleOptionWithNoMatchesData))]
        public void SingleOption_ShouldReturnNone_IfNoItemsMatchThePredicate(
            IEnumerable<int> sequence, Func<int, bool> predicate)
        {
            sequence.SingleOption(predicate).ShouldBe(None);
        }

        public static IEnumerable<object[]> SingleOptionWithNoMatchesData()
        {
            yield return new object[] { Empty<int>(), new Func<int, bool>(_ => true) };
            yield return new object[] { Empty<int>(), new Func<int, bool>(_ => false) };
            yield return new object[] { Range(5, 10), new Func<int, bool>(x => x < 3) };
        }

        [Theory]
        [MemberData(nameof(SingleOptionWithMoreThanOneMatchData))]
        public void SingleOption_ShouldFail_IfMoreThanOneItemMatchesThePredicate(
            IEnumerable<int> sequence, Func<int, bool> predicate)
        {
            Should.Throw<InvalidOperationException>(() => sequence.SingleOption(predicate));
        }

        public static IEnumerable<object[]> SingleOptionWithMoreThanOneMatchData()
        {
            yield return new object[] { Range(5, 10), new Func<int, bool>(x => x > 3) };
        }

        [Theory]
        [MemberData(nameof(ConcatStringsData))]
        public void ConcatStrings_ShouldReturnACorrectConcatenationOfStrings(
            IEnumerable<int> sequence, string expected)
        {
            sequence.ConcatStrings(",", "[", "]").ShouldBe(expected);
        }

        public static IEnumerable<object[]> ConcatStringsData()
        {
            yield return new object[] { Empty<int>(), "[]" };
            yield return new object[] { Range(1, 1), "[1]" };
            yield return new object[] { Range(1, 4), "[1,2,3,4]" };
        }

        [Fact]
        public void Scan_ShouldReturnTheSeedAlone_IfSequenceIsEmpty()
        {
            var next = Substitute.For<Func<string, int, string>>();
            var seed = "";
            Empty<int>().Scan(seed, next).ShouldBe(Items(seed));
        }

        [Fact]
        public void Scan_ShouldReturnTheSequenceOfResultStartingFromTheSeed_IfSequenceIsNotEmpty()
        {
            Range(1, 5).Scan("", (s, n) => s + n).ShouldBe(Items(
                "",
                "1",
                "12",
                "123",
                "1234",
                "12345"));
        }

        [Fact]
        public void ZipScan_ShouldReturnAnEmptySequence_IfSequenceIsEmpty()
        {
            var next = Substitute.For<Func<string, int, string>>();
            Empty<int>().ZipScan("", next).ShouldBe(Empty<(int, string)>());
        }

        [Fact]
        public void ZipScan_ShouldReturnTheSequenceOfResultsPairedWithItems_IfSequenceIsNotEmpty()
        {
            Range(1, 5).ZipScan("", (s, n) => s + n).ShouldBe(Items(
                (1, "1"),
                (2, "12"),
                (3, "123"),
                (4, "1234"),
                (5, "12345")));
        }

        [Fact]
        public void MinMaxOption_ShouldReturnNone_IfSequenceIsEmpty()
        {
            Empty<int>().MaxOption().ShouldBe(None);
            Empty<int>().MinOption().ShouldBe(None);
        }

        [Theory]
        [MemberData(nameof(MinMaxData))]
        public void MinMaxOption_ShouldReturnMinMax_IfSequenceIsNotEmpty(
            IEnumerable<int> sequence, int min, int max)
        {
            sequence.MinOption().ShouldBe(Some(min));
            sequence.MaxOption().ShouldBe(Some(max));
        }

        public record Wrapper(int Value);
        
        [Fact]
        public void MinMaxByOption_ShouldReturnNone_IfSequenceIsEmpty()
        {
            Empty<Wrapper>().MaxByOption(x => x.Value).ShouldBe(None);
            Empty<Wrapper>().MinByOption(x => x.Value).ShouldBe(None);
        }

        [Theory]
        [MemberData(nameof(MinMaxData))]
        public void MinMaxByOption_ShouldReturnMinMax_IfSequenceIsNotEmpty(
            IEnumerable<int> sequence, int min, int max)
        {
            var wrapped = sequence.Select(x => new Wrapper(x));
            wrapped.MinByOption(x => x.Value).ShouldBe(Some(new Wrapper(min)));
            wrapped.MaxByOption(x => x.Value).ShouldBe(Some(new Wrapper(max)));
        }

        [Fact]
        public void MinMaxBy_ShouldFail_IfSequenceIsEmpty()
        {
            Should.Throw<InvalidOperationException>(() => Empty<Wrapper>().MaxBy(x => x.Value));
            Should.Throw<InvalidOperationException>(() => Empty<Wrapper>().MinBy(x => x.Value));
        }

        [Theory]
        [MemberData(nameof(MinMaxData))]
        public void MinMaxBy_ShouldReturnMinMax_IfSequenceIsNotEmpty(
            IEnumerable<int> sequence, int min, int max)
        {
            var wrapped = sequence.Select(x => new Wrapper(x));
            wrapped.MinBy(x => x.Value).ShouldBe(new Wrapper(min));
            wrapped.MaxBy(x => x.Value).ShouldBe(new Wrapper(max));
        }

        public static IEnumerable<object[]> MinMaxData()
        {
            yield return new object[] { Items(1), 1, 1 };
            yield return new object[] { Items(1, 2, 3, 4, 5), 1, 5 };
            yield return new object[] { Items(3, 2, 1, 5, 4), 1, 5 };
        }
    }
}
