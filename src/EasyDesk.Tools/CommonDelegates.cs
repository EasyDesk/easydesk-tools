using EasyDesk.Tools.Results;

namespace EasyDesk.Tools
{
    public delegate T Mapper<T>(T arg);

    public delegate ResultBase<T, E> ResultMapper<T, E>(T arg);
}
