using System;
using Domo.DI;
using Domo.DI.Registration;

namespace Domo.Tests.DI
{
    public class ContainerTests : UnitTests<IContainer>
    {
        private ContainerConfigurationDelegate _containerConfigurationDelegate;

        protected override IContainer CreateTestInstance()
        {
            return Container.Create(_containerConfigurationDelegate);
        }

        protected override void RunTest()
        {
        }

        protected void GivenNoRegistrationIsBeingUsed()
        {
            _containerConfigurationDelegate = (c, r, s) => {};
        }

        protected void GivenThatAnAssemblyScannerIsBeingUsed(Action<IAssemblyScanner> scanner)
        {
            _containerConfigurationDelegate = (c, r, s) => scanner(s);
        }
    }
}