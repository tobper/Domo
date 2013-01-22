using System;
using System.Collections.Generic;
using Domo.Extensions;

namespace Domo.Settings.DI.Activation
{
    public abstract class SettingsActivator
    {
        private readonly IDictionary<Type, IGenericLoader> _genericActivators;

        protected SettingsActivator()
        {
            _genericActivators = new Dictionary<Type, IGenericLoader>();
        }

        protected IGenericLoader GetGenericLoader(Type settingsType)
        {
            return _genericActivators.TryGetValue(settingsType, CreateGenericLoader);
        }

        private static IGenericLoader CreateGenericLoader(Type settingsType)
        {
            var genericLoaderType = typeof(GenericLoader<>).MakeGenericType(settingsType);
            var genericLoader = (IGenericLoader)Activator.CreateInstance(genericLoaderType);

            return genericLoader;
        }

        protected interface IGenericLoader
        {
            object LoadInstance(IApplicationSettings activationContext, string name);
            object LoadInstance(IUserSettings userSettings, string name);
        }

        private class GenericLoader<TSettings> : IGenericLoader
        {
            public object LoadInstance(IApplicationSettings applicationSettings, string name)
            {
                return applicationSettings.Load<TSettings>(name).Result;
            }

            public object LoadInstance(IUserSettings userSettings, string name)
            {
                return userSettings.Load<TSettings>(name).Result;
            }
        }
    }
}