using System;

namespace Domo.DI
{
    public class NoValidConstructorFoundException : Exception
    {
        public Type ClassType { get; private set; }

        public NoValidConstructorFoundException(Type classType)
            : base(CreateMessage(classType))
        {
            ClassType = classType;
        }

        private static string CreateMessage(Type classType)
        {
            return string.Format("No valid constructor could be located for {0}.", classType.Name);
        }
    }
}