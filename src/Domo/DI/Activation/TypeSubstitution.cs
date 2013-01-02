using System;
using System.Collections.Generic;
using Domo.Extensions;

namespace Domo.DI.Activation
{
    public class TypeSubstitution : ITypeSubstitution
    {
        private readonly IDictionary<ServiceIdentity, Type> _types = new Dictionary<ServiceIdentity, Type>();

        public void AddSubstitution(ServiceIdentity identity, Type realServiceType)
        {
            _types.Add(identity, realServiceType);
        }

        public Type TryGetSubstitutedType(ServiceIdentity identity)
        {
            return _types.TryGetValue(identity);
        }
    }
}