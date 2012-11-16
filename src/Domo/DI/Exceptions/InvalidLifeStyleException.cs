using System;

namespace Domo.DI
{
    public class InvalidLifeStyleException : Exception
    {
        public InvalidLifeStyleException(LifeStyle lifeStyle)
            : base(CreateMessage(lifeStyle))
        {
        }

        public InvalidLifeStyleException(LifeStyle lifeStyle, Type serviceType)
            : base(CreateMessage(lifeStyle, serviceType))
        {
        }

        private static string CreateMessage(LifeStyle lifeStyle)
        {
            return string.Format("Invalid life style ({0}).", lifeStyle);
        }

        private static string CreateMessage(LifeStyle lifeStyle, Type serviceType)
        {
            return string.Format("Invalid life style ({0}) specified for service {1}.", lifeStyle, serviceType.Name);
        }
    }
}