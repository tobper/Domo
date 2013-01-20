using Domo.DI.Registration;
using NUnit.Framework;

namespace Domo.Tests.DI
{
    [TestFixture]
    public class When_creating_a_container_using_an_assembly_scanner : ContainerTests
    {
        [Test]
        public void Something_something_should_happen()
        {
        }

        protected override void SetupPrerequisites()
        {
            GivenThatAnAssemblyScannerIsBeingUsed(SetupAssemblyScanner);
        }

        private void SetupAssemblyScanner(IAssemblyScanner scanner)
        {
        }
    }
}