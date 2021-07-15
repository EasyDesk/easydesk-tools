using EasyDesk.Core.Options;
using Shouldly;
using System;
using Xunit;
using static EasyDesk.Core.Options.OptionImports;

namespace EasyDesk.Core.UnitTests.Options
{
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
            OptionImports.AsOption<string>(null).ShouldBe(None);
        }

        [Fact]
        public void AsOptionForReferenceTypes_ShouldReturnANonEmptyOption_WhenANonNullValueIsPassed()
        {
            OptionImports.AsOption("abc").ShouldBe(Some("abc"));
        }

        [Fact]
        public void AsOptionForNullableValueTypes_ShouldReturnAnEmptyOption_WhenNullIsPassed()
        {
            OptionImports.AsOption<int>(null).ShouldBe(None);
        }

        [Fact]
        public void AsOptionForNullableValueTypes_ShouldReturnANonEmptyOption_WhenANonNullValueIsPassed()
        {
            OptionImports.AsOption<int>(1).ShouldBe(Some(1));
        }
    }
}
