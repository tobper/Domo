using System;

namespace Domo.DI.Caching
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class HttpRequestScopeAttribute : ServiceScopeAttribute
    {
        private new static readonly IServiceScope Scope = new HttpRequestScope();

        public HttpRequestScopeAttribute()
            : base(Scope)
        {
        }
    }
}