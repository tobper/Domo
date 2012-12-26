using System;

namespace Domo.DI.Activation
{
    public interface IIdentityManager
    {
        void AddAlias(ServiceIdentity serviceIdentity, Type realServiceType);
        ServiceIdentity GetRealIdentity(ServiceIdentity identity);
    }
}