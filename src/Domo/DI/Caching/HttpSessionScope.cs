using System;
using System.Web;
using Domo.DI.Activation;

namespace Domo.DI.Caching
{
    public class HttpSessionScope : IServiceScope
    {
        private static readonly string SessionKey = typeof(HttpSessionScope).FullName;

        public IServiceCache GetCache(IInjectionContext context)
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
                throw new InvalidOperationException("Todo");

            var session = httpContext.Session;
            if (session == null)
                throw new InvalidOperationException("Todo");

            var cache = (IServiceCache)session[SessionKey];
            if (cache == null)
            {
                cache = new DictionaryServiceCache();
                session[SessionKey] = cache;
            }

            return cache;
        }
    }
}