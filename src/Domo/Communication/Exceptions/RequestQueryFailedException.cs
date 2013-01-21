using System;

namespace Domo.Communication
{
    public class RequestQueryFailedException : MessagingException
    {
        public RequestQueryFailedException(Type resultType)
            : base(CreateMessage(resultType))
        {
        }

        public RequestQueryFailedException(Type resultType, Type queryType)
            : base(CreateMessage(resultType, queryType))
        {
        }

        private static string CreateMessage(Type resultType)
        {
            return string.Format("No query handler for result {0} has been registered.", resultType.Name);
        }

        private static string CreateMessage(Type resultType, Type queryType)
        {
            return string.Format("No query handler for result {0} and query {1} has been registered.", resultType.Name, queryType.Name);
        }
    }
}