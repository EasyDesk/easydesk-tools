using System;
using System.Threading.Tasks;

namespace EasyDesk.Tools.Observables;

public interface IAsyncObservable<out T>
{
    ISubscription Subscribe(Action<T> handler);
}

public static class AsyncObservableExtensions
{
    public static ISubscription Subscribe<T>(this IAsyncObservable<T> observable, System.Action<T> handler)
    {
        return observable.Subscribe(t =>
        {
            handler(t);
            return Task.CompletedTask;
        });
    }
}
