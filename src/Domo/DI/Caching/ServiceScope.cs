using System;

namespace Domo.DI.Caching
{
    public static class ServiceScope
    {
        private static IServiceScope _default;

        static ServiceScope()
        {
            _default = new TransientScope();
        }

        public static IServiceScope Default
        {
            get { return _default; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _default = value;
            }
        }
    }
}