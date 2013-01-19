using System;

namespace Domo.Communication
{
    public class RequestQueryFailedException : MessagingException
    {
        public RequestQueryFailedException(Type queryType, Type resultType)
            : base(CreateMessage(queryType, resultType))
        {
        }

        private static string CreateMessage(Type queryType, Type resultType)
        {
            return string.Format("No query handler for query {0} and result {1} has been registered.", queryType.Name, resultType.Name);
        }
    }
}