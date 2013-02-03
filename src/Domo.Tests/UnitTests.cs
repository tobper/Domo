using System;
using System.Diagnostics;
using System.Transactions;
using NUnit.Framework;

namespace Domo.Tests
{
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