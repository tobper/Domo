using System;

namespace Domo.DI.Caching
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class SingletonScopeAttribute : ServiceScopeAttribute
    {
        private new static readonly IServiceScope Scope = new SingletonScope();

        public SingletonScopeAttribute()
            : base(Scope)
        {
        }
    }
}