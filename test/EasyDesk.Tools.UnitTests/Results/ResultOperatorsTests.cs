using EasyDesk.Tools.Results;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;
using static EasyDesk.Tools.Results.ResultImports;

namespace EasyDesk.Tools.UnitTests.Results;

public class ResultOperatorsTests
{
    private const string TestString = "TEST";

    private static readonly int _value = 10;
    private static readonly Error _error = new TestError(false);
    private static readonly Error _mappedError = new TestError(true);

    private static Result<int> Success => Success(_value);

    private static Result<int> Failure => Failure<int>(_error);

    public static IEnumerable<object[]> AllTypesOfResult()
    {
        yield return new[] { Success };
        yield return new[] { Failure };
    }

    [Fact]
    public void IfSuccess_ShouldNotCallTheGivenFunction_ForFailedResults()
    {
        var shouldNotBeCalled = Substitute.For<Action<int>>();
        Failure.IfSuccess(shouldNotBeCalled);
        shouldNotBeCalled.DidNotReceiveWithAnyArgs()(default);
    }

    [Fact]
    public void IfSuccess_ShouldCallTheGivenFunction_ForSuccessfulResults()
    {
        var shouldBeCalled = Substitute.For<Action<int>>();
        Success.IfSuccess(shouldBeCalled);
        shouldBeCalled.Received(1)(_value);
    }

    [Theory]
    [MemberData(nameof(AllTypesOfResult))]
    public void IfSuccess_ShouldReturnTheSameResult(Result<int> result)
    {
        result.IfSuccess(Substitute.For<Action<int>>()).ShouldBe(result);
    }

    [Fact]
    public void IfFailure_ShouldCallTheGivenFunction_ForFailedResults()
    {
        var shouldBeCalled = Substitute.For<Action<Error>>();
        Failure.IfFailure(shouldBeCalled);
        shouldBeCalled.Received(1)(_error);
    }

    [Fact]
    public void IfFailure_ShouldNotCallTheGivenFunction_ForFailedResults()
    {
        var shouldNotBeCalled = Substitute.For<Action<Error>>();
        Success.IfFailure(shouldNotBeCalled);
        shouldNotBeCalled.DidNotReceiveWithAnyArgs()(default);
    }

    [Theory]
    [MemberData(nameof(AllTypesOfResult))]
    public void IfFailure_ShouldReturnTheSameResult(Result<int> result)
    {
        result.IfFailure(Substitute.For<Action<Error>>()).ShouldBe(result);
    }

    [Fact]
    public void Map_ShouldMapTheWrappedValue_ForSuccessfulResults()
    {
        var mapper = Substitute.For<Func<int, string>>();
        mapper(_value).Returns(TestString);
        Success.Map(mapper).ShouldBe(Success(TestString));
        mapper.Received(1)(_value);
    }

    [Fact]
    public void Map_ShouldReturnTheSameError_ForFailedResults()
    {
        var mapper = Substitute.For<Func<int, string>>();
        Failure.Map(mapper).ShouldBe(Failure<string>(_error));
        mapper.DidNotReceiveWithAnyArgs()(default);
    }

    [Fact]
    public void MapError_ShouldMapTheWrappedError_ForFailedResults()
    {
        var mapper = Substitute.For<Func<Error, Error>>();
        mapper(_error).Returns(_mappedError);
        Failure.MapError(mapper).ShouldBe(Failure<int>(_mappedError));
        mapper.Received(1)(_error);
    }

    [Fact]
    public void MapError_ShouldReturnTheSameValue_ForSuccessfulResults()
    {
        var mapper = Substitute.For<Func<Error, Error>>();
        Success.MapError(mapper).ShouldBe(Success);
        mapper.DidNotReceiveWithAnyArgs()(default);
    }

    [Fact]
    public void FlatMap_ShouldReturnTheSameError_ForFailedResults()
    {
        var mapper = Substitute.For<Func<int, Result<string>>>();
        Failure.FlatMap(mapper).ShouldBe(Failure<string>(_error));
        mapper.DidNotReceiveWithAnyArgs()(_value);
    }

    [Theory]
    [MemberData(nameof(FlatMapData))]
    public void FlatMap_ShouldReturnTheMappedValue_ForSuccessfulResults(
        Result<string> mappedResult)
    {
        var mapper = Substitute.For<Func<int, Result<string>>>();
        mapper(_value).Returns(mappedResult);
        Success.FlatMap(mapper).ShouldBe(mappedResult);
        mapper.Received(1)(_value);
    }

    public static IEnumerable<object[]> FlatMapData()
    {
        yield return new[] { Success(TestString) };
        yield return new[] { Failure<string>(_mappedError) };
    }

    [Fact]
    public void FlatTap_ShouldReturnTheSameError_ForFailedResults()
    {
        var mapper = Substitute.For<Func<int, Result<string>>>();
        Failure.FlatTap(mapper).ShouldBe(Failure);
        mapper.DidNotReceiveWithAnyArgs()(default);
    }

    [Fact]
    public void FlatTap_ShouldReturnTheMappedError_ForSuccessfulResults()
    {
        var mapper = Substitute.For<Func<int, Result<string>>>();
        mapper(_value).Returns(Failure<string>(_mappedError));
        Success.FlatTap(mapper).ShouldBe(Failure<int>(_mappedError));
        mapper.Received(1)(_value);
    }

    [Fact]
    public void FlatTap_ShouldReturnTheMappedValue_ForSuccessfulResults()
    {
        var mapper = Substitute.For<Func<int, Result<string>>>();
        mapper(_value).Returns(Success(TestString));
        Success.FlatTap(mapper).ShouldBe(Success);
        mapper.Received(1)(_value);
    }
}
