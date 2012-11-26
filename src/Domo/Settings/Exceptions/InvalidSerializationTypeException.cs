using System;

namespace Domo.Settings
{
    public class InvalidSerializationTypeException : Exception
    {
        public InvalidSerializationTypeException()
            : base("The registered provider does not support the storage type of the registered serializer.")
        {
        }
    }
}