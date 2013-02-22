using System;

namespace Domo.DI.Caching
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class HttpSessionScopeAttribute : ServiceScopeAttribute
    {
        private new static readonly IServiceScope Scope = new HttpSessionScope();

        public HttpSessionScopeAttribute()
            : base(Scope)
        {
        }
    }
}