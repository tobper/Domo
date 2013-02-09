using System;
using System.Web;
using Domo.DI;
using Domo.DI.Registration;

namespace Domo.Tests.DI
{
    public class ContainerTests : UnitTests<IContainer>
    {
        private Action<IContainerConfiguration> _registrationDelegate;
        private Action<IAssemblyScanner> _scannerDelegate;

        protected override IContainer CreateTestInstance()
        {
            return Container.Create(_registrationDelegate, _scannerDelegate);
        }

        protected override void RunTest()
        {
        }

        protected void GivenNoRegistrationIsBeingUsed()
        {
            _scannerDelegate = null;
        }

        protected void GivenThatAnAssemblyScannerIsBeingUsed(Action<IAssemblyScanner> scanner)
        {
            _scannerDelegate = scanner;
        }
    }
}