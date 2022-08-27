namespace EasyDesk.Tools.Collections;

public static class AsyncEnumerableUtils
{
    public static async Task ForEach<T>(this IAsyncEnumerable<T> sequence, AsyncAction<T> action)
    {
        await foreach (var item in sequence)
        {
            await action(item);
        }
    }

    public static async Task ForEach<T>(this IAsyncEnumerable<T> sequence, Action<T> action)
    {
        await foreach (var item in sequence)
        {
            action(item);
        }
    }

    public static async IAsyncEnumerable<R> Select<T, R>(this IAsyncEnumerable<T> sequence, Func<T, R> mapper)
    {
        await foreach (var item in sequence)
        {
            yield return mapper(item);
        }
    }

    public static async IAsyncEnumerable<T> Where<T>(this IAsyncEnumerable<T> sequence, Func<T, bool> predicate)
    {
        await foreach (var item in sequence)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }

    public static async IAsyncEnumerable<R> SelectMany<T, R>(this IAsyncEnumerable<T> sequence, Func<T, IAsyncEnumerable<R>> mapper)
    {
        await foreach (var item in sequence)
        {
            await foreach (var mapped in mapper(item))
            {
                yield return mapped;
            }
        }
    }

    public static IAsyncEnumerable<T> Create<T>(Func<CancellationToken, IAsyncEnumerator<T>> enumeratorProvider) =>
        new AsyncEnumerableImpl<T>(enumeratorProvider);

    private sealed class AsyncEnumerableImpl<T> : IAsyncEnumerable<T>
    {
        private readonly Func<CancellationToken, IAsyncEnumerator<T>> _enumeratorProvider;

        public AsyncEnumerableImpl(Func<CancellationToken, IAsyncEnumerator<T>> enumeratorProvider)
        {
            _enumeratorProvider = enumeratorProvider;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            _enumeratorProvider(cancellationToken);
    }

    public static IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IEnumerable<T> sequence) =>
        Create(_ => new AsyncEnumeratorFromEnumerator<T>(sequence.GetEnumerator()));

    private sealed class AsyncEnumeratorFromEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public AsyncEnumeratorFromEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(_enumerator.MoveNext());

        public T Current => _enumerator.Current;

        public ValueTask DisposeAsync()
        {
            _enumerator.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
