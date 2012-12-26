using System;
using Domo.DI;
using Domo.DI.Registration;

namespace Domo.Tests.DI
{
    public class ContainerTests : UnitTests<IContainer>
    {
        private Action<ITypeRegistration> _typeRegistration;

        protected override IContainer CreateTestInstance()
        {
            return Container.Create(_typeRegistration);
        }

        protected override void RunTest()
        {
        }

        protected void GivenNoRegistrationIsBeingUsed()
        {
            _typeRegistration = null;
        }

        protected void GivenThatAnAssemblyScannerIsBeingUsed()
        {
        }
    }
}