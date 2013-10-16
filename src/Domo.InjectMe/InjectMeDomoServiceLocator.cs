using InjectMe;

namespace Domo
{
    public class InjectMeDomoServiceLocator : IDomoServiceLocator
    {
        private readonly IServiceLocator _serviceLocator;

        public InjectMeDomoServiceLocator(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public T TryResolve<T>()
            where T : class
        {
            return _serviceLocator.TryResolve<T>();
        }

        public T[] TryResolveAll<T>()
            where T : class
        {
            return _serviceLocator.TryResolveAll<T>();
        }
    }
}