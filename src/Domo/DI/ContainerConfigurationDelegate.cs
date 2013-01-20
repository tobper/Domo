using Domo.DI.Registration;

namespace Domo.DI
{
    public delegate void ContainerConfigurationDelegate(IContainer container, ITypeRegistration registration, IAssemblyScanner scanner);
}