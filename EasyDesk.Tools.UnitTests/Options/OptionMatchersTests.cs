using EasyDesk.Tools.Options;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;
using static EasyDesk.Tools.Options.OptionImports;

namespace EasyDesk.Tools.UnitTests.Options
{
    public class OptionMatchersTests
    {
        private const int _value = 5;
        private const int _other = 10;

        private const string _reference = "abc";

        [Fact]
        public void OrElse_ShouldReturnTheGivenValue_IfOptionIsEmpty()
        {
            NoneT<int>().OrElse(_other).ShouldBe(_other);
        }

        [Fact]
        public void OrElse_ShouldReturnTheValueInsideTheOption_IfOptionIsNotEmpty()
        {
            Some(_value).OrElse(_other).ShouldBe(_value);
        }

        [Fact]
        public void OrElseGet_ShouldReturnTheValueReturnedByTheGivenFunction_IfOptionIsEmpty()
        {
            NoneT<int>().OrElseGet(() => _other).ShouldBe(_other);
        }

        [Fact]
        public void OrElseGet_ShouldReturnTheValueInsideTheOption_IfOptionIsNotEmpty()
        {
            Some(_value).OrElseGet(() => _other).ShouldBe(_value);
        }

        [Fact]
        public async Task OrElseGetAsync_ShouldReturnTheGivenTask_IfOptionIsEmpty()
        {
            var result = await NoneT<int>().OrElseGetAsync(() => Task.FromResult(_other));
            result.ShouldBe(_other);
        }

        [Fact]
        public async Task OrElseGetAsync_ShouldReturnTheValueInsideTheOption_IfOptionIsNotEmpty()
        {
            var result = await Some(_value).OrElseGetAsync(() => Task.FromResult(_other));
            result.ShouldBe(_value);
        }

        [Fact]
        public void OrElseDefault_ShouldReturnTheDefaultValueForTheType_IfOptionIsEmpty()
        {
            NoneT<int>().OrElseDefault().ShouldBe(0);
        }

        [Fact]
        public void OrElseDefault_ShouldReturnTheValueInsideTheOption_IfOptionIsNotEmpty()
        {
            Some(_value).OrElseDefault().ShouldBe(_value);
        }

        [Fact]
        public void OrElseNull_ShouldReturnNull_IfOptionIsEmpty()
        {
            NoneT<string>().OrElseNull().ShouldBeNull();
        }

        [Fact]
        public void OrElseNull_ShouldReturnTheValueInsideTheOption_IfOptionIsNotEmpty()
        {
            Some(_reference).OrElseNull().ShouldBe(_reference);
        }

        [Fact]
        public void OrElseThrow_ShouldThrowTheGivenException_IfOptionIsEmpty()
        {
            Should.Throw<Exception>(() => NoneT<int>().OrElseThrow(() => new Exception()));
        }

        [Fact]
        public void OrElseThrow_ShouldReturnTheValueInsideTheOption_IfOptionIsNotEmpty()
        {
            Some(_value).OrElseThrow(() => new Exception()).ShouldBe(_value);
        }

        [Fact]
        public void AsNullable_ShouldReturnNull_IfOptionIsEmpty()
        {
            Some(_value).AsNullable().ShouldBe(_value);
        }

        [Fact]
        public void AsNullable_ShouldReturnTheValueInsideTheOption_IfOptionIsNotEmpty()
        {
            NoneT<int>().AsNullable().ShouldBeNull();
        }
    }
}
