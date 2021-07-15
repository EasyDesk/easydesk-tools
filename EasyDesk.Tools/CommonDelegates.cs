using EasyDesk.Core.Results;

namespace EasyDesk.Core
{
    public delegate T Mapper<T>(T arg);

    public delegate ResultBase<T, E> ResultMapper<T, E>(T arg);
}
