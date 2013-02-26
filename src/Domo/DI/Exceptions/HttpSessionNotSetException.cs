using System;

namespace Domo.DI
{
    public class HttpSessionNotSetException : Exception
    {
        public HttpSessionNotSetException()
            : base("The current HttpContext does not have a session.")
        {
        }
    }
}