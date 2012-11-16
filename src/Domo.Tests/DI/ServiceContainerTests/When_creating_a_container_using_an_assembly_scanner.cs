using NUnit.Framework;

namespace Domo.Tests.DI.ServiceContainerTests
{
    [TestFixture]
    public class When_creating_a_container_using_an_assembly_scanner : ServiceContainerTests
    {
        [Test]
        public void Something_something_should_happen()
        {
        }

        protected override void SetupPrerequisites()
        {
            GivenThatAnAssemblyScannerIsBeingUsed();
        }
    }
}