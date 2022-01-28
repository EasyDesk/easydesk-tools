﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyDesk.Tools.Observables;

public class SimpleAsyncEvent<T> : IAsyncObservable<T>, IAsyncEmitter<T>
{
    private readonly List<Action<T>> _handlers = new();

    public async Task Emit(T value)
    {
        foreach (var handler in _handlers)
        {
            await handler(value);
        }
    }

    public ISubscription Subscribe(Action<T> handler)
    {
        _handlers.Add(handler);
        return new SimpleSubscription(() => _handlers.Remove(handler));
    }
}
