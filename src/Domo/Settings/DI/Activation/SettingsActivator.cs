using System;
using System.Collections.Generic;
using Domo.Extensions;

namespace Domo.Settings.DI.Activation
{
    public abstract class SettingsActivator
    {
        private readonly IDictionary<Type, IGenericActivator> _genericActivators;

        protected SettingsActivator()
        {
            _genericActivators = new Dictionary<Type, IGenericActivator>();
        }

        protected IGenericActivator GetGenericActivator(Type settingsType)
        {
            return _genericActivators.TryGetValue(settingsType, CreateGenericActivator);
        }

        private static IGenericActivator CreateGenericActivator(Type settingsType)
        {
            var genericActivatorType = typeof(GenericActivator<>).MakeGenericType(settingsType);
            var genericActivator = (IGenericActivator)Activator.CreateInstance(genericActivatorType);

            return genericActivator;
        }

        protected interface IGenericActivator
        {
            object ActivateInstance(IApplicationSettings activationContext, string name);
            object ActivateInstance(IUserSettings userSettings, string name);
        }

        private class GenericActivator<TSettings> : IGenericActivator
        {
            public object ActivateInstance(IApplicationSettings applicationSettings, string name)
            {
                return applicationSettings.Load<TSettings>(name);
            }

            public object ActivateInstance(IUserSettings userSettings, string name)
            {
                return userSettings.Load<TSettings>(name);
            }
        }
    }
}