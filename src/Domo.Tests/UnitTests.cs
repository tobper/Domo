using System;
using System.Diagnostics;
using System.Transactions;
using Domo.DI;
using Domo.Settings;
using Domo.Settings.DI;
using Domo.Settings.ProviderBasedSettings.Serialization;
using Domo.Settings.ProviderBasedSettings.Storage;
using NUnit.Framework;
using Domo.Settings.DI.Registration;

namespace Domo.Tests
{
    //[TestFixture]
    //public class Beep
    //{
    //    private const string ConnectionString = "Server=tcp:o1d6jmfcb8.database.windows.net,1433;Database=DiveLog;User ID=DiveLog@o1d6jmfcb8;Password=4P2BqJyoKxrf8Pt2mnb6;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
    //    private IContainer _container;

    //    [TestFixtureSetUp]
    //    public void Setup()
    //    {
    //        _container = Container.Create(
    //            r => r.
    //                RegisterSingleton(ConnectionString, "SqlServerSettingsConnection").
    //                RegisterProviderBasedSettings<JsonSettingsSerializer, SqlServerSettingsStorageProvider, PrincipalSettingsUsernameProvider>(),
    //            s => s.
    //                UseConventionBasedProcessor().
    //                UseSettingsProcessor().
    //                ScanAssemblyContaining<Setting>().
    //                ScanAssemblyContaining<Beep>());
    //    }

    //    [Test]
    //    public void TestContainer()
    //    {
    //        var foo = _container.ServiceLocator.Resolve<IFoo>();

    //        Assert.IsNotNull(foo);
    //    }

    //    [Test]
    //    [Ignore]
    //    public void TestSettings()
    //    {
    //        var userSettings = _container.ServiceLocator.Resolve<IUserSettings>();

    //        userSettings.Save(new UserProfile
    //        {
    //            Name = "Kalle"
    //        });

    //        var userProfile = userSettings.Load<UserProfile>();

    //        Assert.IsNotNull(userProfile);
    //    }
    //}

    //public interface IFoo
    //{
        
    //}

    //public class Foo : IFoo
    //{
    //    private readonly UserProfile _userProfile;

    //    public Foo(UserProfile userProfile)
    //    {
    //        _userProfile = userProfile;
    //    }
    //}

    //[UserSettings]
    //public class UserProfile
    //{
    //    public string Name { get; set; }
    //}

    [DebuggerStepThrough]
    public abstract class UnitTests<TTestInstance>
    {
        protected Type ExpectedException { get; set; }
        protected TTestInstance TestInstance { get; private set; }
        protected Exception ThrownException { get; private set; }

        [SetUp]
        public void Setup()
        {
            using (new TransactionScope())
            {
                ExpectedException = GetExpectedException();
                CreateMocks();
                SetupPrerequisites();
                SetupBehavior();

                try
                {
                    TestInstance = CreateTestInstance();
                    RunTest();
                    GetTestResult();
                }
                catch (Exception e)
                {
                    ThrownException = e;

                    if (e.GetType() != ExpectedException)
                        throw;
                }
            }
        }

        [TearDown]
        public void Cleanup()
        {
            CleanupAfterTest();
        }

        protected virtual Type GetExpectedException()
        {
            return null;
        }

        protected virtual void CreateMocks()
        {
        }

        protected virtual void SetupPrerequisites()
        {
        }

        protected virtual void SetupBehavior()
        {
        }

        protected abstract TTestInstance CreateTestInstance();

        protected abstract void RunTest();

        protected virtual void GetTestResult()
        {
        }

        protected virtual void CleanupAfterTest()
        {
        }
    }
}