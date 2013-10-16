namespace Domo
{
    public interface IDomoServiceLocator
    {
        T TryResolve<T>() where T : class;
        T[] TryResolveAll<T>() where T : class;
    }
}