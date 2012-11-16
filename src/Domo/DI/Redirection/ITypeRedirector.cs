using System;

namespace Domo.DI.Redirection
{
    public interface ITypeRedirector
    {
        void AddRedirection(Type serviceType, string serviceName, Type instanceType);
        Type GetRedirection(Type serviceType, string serviceName);
    }
}