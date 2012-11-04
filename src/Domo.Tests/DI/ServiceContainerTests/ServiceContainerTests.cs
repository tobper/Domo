using System;
using Domo.DI;
using Domo.DI.Scanning;

namespace Domo.Tests.DI.ServiceContainerTests
{
    public class ServiceContainerTests : UnitTests<IServiceContainer>
    {
        private Action<IServiceRegistration> _serviceRegistration;

        protected override IServiceContainer CreateTestInstance()
        {
            return ServiceContainer.Create(_serviceRegistration);
        }

        protected override void PerformTest()
        {
        }

        protected void GivenNoRegistrationIsBeingUsed()
        {
            _serviceRegistration = null;
        }

        protected void GivenThatAnAssemblyScannerIsBeingUsed()
        {
        }
    }
}