using EasyDesk.Core.Collections;
using EasyDesk.Core.Observables;
using NSubstitute;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EasyDesk.Core.UnitTests.Observables
{
    public class SimpleAsyncEventTests
    {
        private const int _value = 0;

        private readonly SimpleAsyncEvent<int> _sut = new();

        [Fact]
        public async Task Emit_ShouldNotFail_IfEventHasNoSubscriber()
        {
            await Should.NotThrowAsync(() => _sut.Emit(_value));
        }

        [Fact]
        public async Task Emit_ShouldNotifyHandlersWithTheGivenValueAsync()
        {
            var handler1 = Substitute.For<Action<int>>();
            var handler2 = Substitute.For<Action<int>>();
            _sut.Subscribe(handler1);
            _sut.Subscribe(handler2);

            await _sut.Emit(_value);

            handler1.Received(1)(_value);
            handler2.Received(1)(_value);
        }

        [Fact]
        public async Task Emit_ShouldNotifyAllHandlersInOrderOfSubscriptionAsync()
        {
            var index = 0;

            void InOrderHandler(int expected)
            {
                index.ShouldBe(expected);
                index++;
            }

            Enumerable.Range(0, 10).ForEach(i =>
            {
                _sut.Subscribe(_ => InOrderHandler(i));
            });

            await _sut.Emit(0);
        }

        [Fact]
        public async Task Emit_ShouldNotNotifyUnsubscribedHandlers()
        {
            var handler = Substitute.For<Action<int>>();
            var subscription = _sut.Subscribe(handler);
            subscription.Unsubscribe();

            await _sut.Emit(_value);

            handler.DidNotReceiveWithAnyArgs()(default);
        }

        [Fact]
        public void Unsubscribe_ShouldFail_IfCalledMultipleTimes()
        {
            var subscription = _sut.Subscribe(_ => { });
            subscription.Unsubscribe();

            Should.Throw<InvalidOperationException>(() => subscription.Unsubscribe());
        }
    }
}
