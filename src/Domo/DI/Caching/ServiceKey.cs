using System;

namespace Domo.DI.Caching
{
    public class ServiceKey
    {
        public Type ServiceType { get; private set; }
        public string ServiceName { get; private set; }

        public ServiceKey(Type serviceType, string serviceName)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            ServiceType = serviceType;
            ServiceName = serviceName;
        }

        public override int GetHashCode()
        {
            var hashcode = ServiceType.GetHashCode();

            if (ServiceName != null)
                hashcode ^= ServiceName.ToLowerInvariant().GetHashCode();

            return hashcode;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ServiceKey;
            if (other == null)
                return false;

            if (ServiceType != other.ServiceType)
                return false;

            if (ServiceName == null && other.ServiceName == null)
                return true;

            if (ServiceName == null || other.ServiceName == null)
                return false;

            return ServiceName.Equals(other.ServiceName, StringComparison.OrdinalIgnoreCase);
        }
    }
}