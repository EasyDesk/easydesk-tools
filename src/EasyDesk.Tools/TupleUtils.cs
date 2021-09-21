using System;

namespace EasyDesk.Tools
{
    public static class TupleUtils
    {
        public static (A2, B2) Map<A1, B1, A2, B2>(this (A1, B1) tuple, Func<A1, A2> mapper1, Func<B1, B2> mapper2) =>
            (mapper1(tuple.Item1), mapper2(tuple.Item2));
    }
}
