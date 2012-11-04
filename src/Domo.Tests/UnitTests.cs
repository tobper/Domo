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
                SetupGivens();
                SetupBehavior();
                TestInstance = CreateTestInstance();
                RunTest();
                GetTestResult();
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

        protected virtual void SetupGivens()
        {
        }

        protected virtual void SetupBehavior()
        {
        }

        protected abstract TTestInstance CreateTestInstance();

        protected abstract void PerformTest();

        protected virtual void GetTestResult()
        {
        }

        protected virtual void CleanupAfterTest()
        {
        }

        private void RunTest()
        {
            try
            {
                PerformTest();
            }
            catch (Exception e)
            {
                ThrownException = e;

                if (e.GetType() != ExpectedException)
                    throw;
            }
        }
    }
}