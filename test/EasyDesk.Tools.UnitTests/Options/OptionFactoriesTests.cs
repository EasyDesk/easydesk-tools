using Shouldly;
using Xunit;

namespace EasyDesk.Tools.UnitTests.Options;

public class OptionFactoriesTests
{
    [Fact]
    public void Some_ShouldFail_WhenNullIsPassed()
    {
        Should.Throw<ArgumentNullException>(() => Some<string>(null));
    }

    [Fact]
    public void AsOptionForReferenceTypes_ShouldReturnAnEmptyOption_WhenNullIsPassed()
    {
        StaticImports.AsOption<string>(null).ShouldBeEmpty();
    }

    [Fact]
    public void AsOptionForReferenceTypes_ShouldReturnANonEmptyOption_WhenANonNullValueIsPassed()
    {
        StaticImports.AsOption("abc").ShouldBe(Some("abc"));
    }

    [Fact]
    public void AsOptionForNullableValueTypes_ShouldReturnAnEmptyOption_WhenNullIsPassed()
    {
        StaticImports.AsOption<int>(null).ShouldBe(None);
    }

    [Fact]
    public void AsOptionForNullableValueTypes_ShouldReturnANonEmptyOption_WhenANonNullValueIsPassed()
    {
        StaticImports.AsOption<int>(1).ShouldBe(Some(1));
    }

    [Fact]
    public void ToOptionForReferenceTypes_ShouldReturnAnEmptyOption_WhenNullIsPassed()
    {
        StaticImports.ToOption<string>(null).ShouldBeEmpty();
    }

    [Fact]
    public void ToOptionForReferenceTypes_ShouldReturnANonEmptyOption_WhenANonNullValueIsPassed()
    {
        StaticImports.ToOption("abc").ShouldBe(Some("abc"));
    }

    [Fact]
    public void ToOptionForNullableValueTypes_ShouldReturnANonEmptyOption_WhenANonNullValueIsPassed()
    {
        StaticImports.ToOption<int>(1).ShouldBe(Some(1));
    }

#nullable enable
    [Fact]
    public void AsOptionForNullableValueTypes_ShouldReturnAnEmptyOption_WhenNullIsPassed_WithNullableEnabled()
    {
        int? x = null;
        StaticImports.AsOption(x).ShouldBe(None);
    }

    [Fact]
    public void ToOptionForNullableValueTypes_ShouldReturnAnEmptyOption_WhenNullIsPassed_WithNullableEnabled()
    {
        int? x = null;
        StaticImports.ToOption(x).ShouldBe(None);
    }
#nullable disable
}
