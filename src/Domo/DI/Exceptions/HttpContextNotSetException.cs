using System;

namespace Domo.DI
{
    public class HttpContextNotSetException : Exception
    {
        public HttpContextNotSetException()
            : base("Could not get current HttpContext.")
        {
        }
    }
}