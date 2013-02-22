using System;

namespace Domo.DI.Caching
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TransientScopeAttribute : ServiceScopeAttribute
    {
        private new static readonly IServiceScope Scope = new TransientScope();

        public TransientScopeAttribute()
            : base(Scope)
        {
        }
    }
}