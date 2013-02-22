using System;
using System.Collections.Generic;

namespace Domo.DI.Activation
{
    public interface IInjectionContext : IDisposable
    {
        IContainer Container { get; }
        IDictionary<object, object> Data { get; }
    }
}