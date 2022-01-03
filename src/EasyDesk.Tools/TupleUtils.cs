using System;

namespace EasyDesk.Tools;

public static class TupleUtils
{
    public static (A2 A, B2 B) Map<A1, B1, A2, B2>(this (A1 A, B1 B) tuple, Func<A1, A2> mapper1, Func<B1, B2> mapper2) =>
        (mapper1(tuple.A), mapper2(tuple.B));
}
