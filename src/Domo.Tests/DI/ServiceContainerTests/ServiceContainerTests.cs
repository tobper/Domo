using System;
using Domo.DI;
using Domo.DI.Registration;

namespace Domo.Tests.DI.ServiceContainerTests
{
    public class ServiceContainerTests : UnitTests<IContainer>
    {
        private Action<ITypeRegistration> _serviceRegistration;

        protected override IContainer CreateTestInstance()
        {
            return Container.Create(_serviceRegistration);
        }

        protected override void RunTest()
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