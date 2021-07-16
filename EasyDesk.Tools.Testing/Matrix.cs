using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static EasyDesk.Tools.Collections.EnumerableUtils;

namespace EasyDesk.Tools.Testing
{
    public static class Matrix
    {
        public static MatrixBuilder<T> Axis<T>(IEnumerable<T> axis) => new(axis);

        public static MatrixBuilder<T> Axis<T>(params T[] axis) => Axis(axis.AsEnumerable());

        public static MatrixBuilder<T> Fixed<T>(T value) => new(Items(value));
    }

    public delegate IEnumerable Expansion(IImmutableList<object> currentParams);

    public abstract class MatrixBuilderBase<T, TBuilder>
        where TBuilder : MatrixBuilderBase<T, TBuilder>
    {
        protected IImmutableStack<Expansion> _expansions;

        public MatrixBuilderBase(IImmutableStack<Expansion> expansions)
        {
            _expansions = expansions;
        }

        private T ConvertToTuple(IImmutableList<object> paramList)
        {
            var enumerator = paramList.GetEnumerator();
            object Next()
            {
                enumerator.MoveNext();
                return enumerator.Current;
            }
            return AsTuple(Next);
        }

        protected abstract T AsTuple(Func<object> next);

        protected IImmutableStack<Expansion> AddExpansion<T2>(Func<T, IEnumerable<T2>> axis) =>
            _expansions.Push(ps => axis(ConvertToTuple(ps)));

        public TBuilder Filter(Func<T, bool> predicate)
        {
            _expansions = _expansions.Pop(out var expansion);
            _expansions = _expansions.Push(ps => FilteredExpansion(ps, expansion, predicate));
            return this as TBuilder;
        }

        private IEnumerable FilteredExpansion(IImmutableList<object> currentParams, Expansion expansion, Func<T, bool> predicate)
        {
            foreach (var p in expansion(currentParams))
            {
                var tuple = ConvertToTuple(currentParams.Add(p));
                if (predicate(tuple))
                {
                    yield return p;
                }
            }
        }

        public IEnumerable<object[]> Build()
        {
            var result = new List<object[]>();
            var stack = new Stack<object>();

            BuildResult(_expansions.ToArray(), stack, result);

            return result;
        }

        private void BuildResult(Expansion[] expansions, Stack<object> currentParams, List<object[]> result)
        {
            if (currentParams.Count == expansions.Length)
            {
                result.Add(currentParams.Reverse().ToArray());
                return;
            }
            var expansionIndex = expansions.Length - currentParams.Count - 1;
            var expansion = expansions[expansionIndex];
            var nextParams = expansion(currentParams.ToImmutableList());
            foreach (var param in nextParams)
            {
                currentParams.Push(param);
                BuildResult(expansions, currentParams, result);
                currentParams.Pop();
            }
        }
    }

    public class MatrixBuilder<T1> : MatrixBuilderBase<T1, MatrixBuilder<T1>>
    {
        public MatrixBuilder(IEnumerable<T1> axis) : base(ImmutableStack.Create<Expansion>(_ => axis.Cast<object>()))
        {

        }

        public MatrixBuilder<T1, T2> Axis<T2>(IEnumerable<T2> axis) => GeneratedAxis(_ => axis);

        public MatrixBuilder<T1, T2> Axis<T2>(params T2[] axis) => Axis(axis.AsEnumerable());

        public MatrixBuilder<T1, T2> GeneratedAxis<T2>(Func<T1, IEnumerable<T2>> axis) => new(AddExpansion(axis));

        public MatrixBuilder<T1, T2> GeneratedValue<T2>(Func<T1, T2> value) => GeneratedAxis(x => Items(value(x)));

        public MatrixBuilder<T1, T2> Fixed<T2>(T2 value) => GeneratedValue(_ => value);

        protected override T1 AsTuple(Func<object> next) => (T1) next();
    }

    public class MatrixBuilder<T1, T2> : MatrixBuilderBase<(T1, T2), MatrixBuilder<T1, T2>>
    {
        public MatrixBuilder(IImmutableStack<Expansion> expansions) : base(expansions)
        {

        }

        public MatrixBuilder<T1, T2, T3> Axis<T3>(IEnumerable<T3> axis) => GeneratedAxis(_ => axis);

        public MatrixBuilder<T1, T2, T3> Axis<T3>(params T3[] axis) => Axis(axis.AsEnumerable());

        public MatrixBuilder<T1, T2, T3> GeneratedAxis<T3>(Func<(T1, T2), IEnumerable<T3>> axis) => new(AddExpansion(axis));

        public MatrixBuilder<T1, T2, T3> GeneratedValue<T3>(Func<(T1, T2), T3> value) => GeneratedAxis(x => Items(value(x)));

        public MatrixBuilder<T1, T2, T3> Fixed<T3>(T3 value) => GeneratedValue(_ => value);

        protected override (T1, T2) AsTuple(Func<object> next) => ((T1) next(), (T2) next());
    }

    public class MatrixBuilder<T1, T2, T3> : MatrixBuilderBase<(T1, T2, T3), MatrixBuilder<T1, T2, T3>>
    {
        public MatrixBuilder(IImmutableStack<Expansion> expansions) : base(expansions)
        {

        }

        public MatrixBuilder<T1, T2, T3, T4> Axis<T4>(IEnumerable<T4> axis) => GeneratedAxis(_ => axis);

        public MatrixBuilder<T1, T2, T3, T4> Axis<T4>(params T4[] axis) => Axis(axis.AsEnumerable());

        public MatrixBuilder<T1, T2, T3, T4> GeneratedAxis<T4>(Func<(T1, T2, T3), IEnumerable<T4>> axis) => new(AddExpansion(axis));

        public MatrixBuilder<T1, T2, T3, T4> GeneratedValue<T4>(Func<(T1, T2, T3), T4> value) => GeneratedAxis(x => Items(value(x)));

        public MatrixBuilder<T1, T2, T3, T4> Fixed<T4>(T4 value) => GeneratedValue(_ => value);

        protected override (T1, T2, T3) AsTuple(Func<object> next) => ((T1) next(), (T2) next(), (T3) next());
    }

    public class MatrixBuilder<T1, T2, T3, T4> : MatrixBuilderBase<(T1, T2, T3, T4), MatrixBuilder<T1, T2, T3, T4>>
    {
        public MatrixBuilder(IImmutableStack<Expansion> expansions) : base(expansions)
        {

        }

        protected override (T1, T2, T3, T4) AsTuple(Func<object> next) => ((T1) next(), (T2) next(), (T3) next(), (T4) next());
    }
}
