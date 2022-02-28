using EasyDesk.Tools.Results;
using Shouldly;
using Xunit;
using static EasyDesk.Tools.Results.ResultImports;

namespace EasyDesk.Tools.UnitTests.Results;

public class ResultFactoriesTests
{
    private readonly Error _error = new TestError(false);

    [Fact]
    public void OkShouldBeSuccessful()
    {
        Ok.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Ensure_ShouldReturnOk_IfTheConditionIsMet()
    {
        Ensure(true, otherwise: () => _error).ShouldBe(Ok);
    }

    [Fact]
    public void Ensure_ShouldReturnTheGivenError_IfTheConditionIsNotMet()
    {
        Ensure(false, otherwise: () => _error).ShouldBe(Failure<Nothing>(_error));
    }

    [Fact]
    public void EnsureNot_ShouldReturnOk_IfTheConditionIsNotMet()
    {
        EnsureNot(false, otherwise: () => _error).ShouldBe(Ok);
    }

    [Fact]
    public void EnsureNot_ShouldReturnTheGivenError_IfTheConditionIsMet()
    {
        EnsureNot(true, otherwise: () => _error).ShouldBe(Failure<Nothing>(_error));
    }
}
