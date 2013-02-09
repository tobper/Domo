using System;
using Moq;

namespace Domo.Tests
{
    public class TestBase
    {
        protected static Mock<T> CreateMock<T>()
            where T : class
        {
            return new Mock<T>(MockBehavior.Strict);
        }

        protected static Mock<T> CreateMock<T>(Action<Mock<T>> setup)
            where T : class
        {
            var mock = CreateMock<T>();

            setup(mock);

            return mock;
        }
    }
}