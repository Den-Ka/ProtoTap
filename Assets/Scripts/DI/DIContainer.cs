using System;
using System.Collections.Generic;

namespace Etern0nety.DI
{
    public class DIContainer : IDisposable
    {
        private readonly Dictionary<Type, DIEntry> _register = new();

        public void RegisterInstance<TType>(TType instance)
        {
            RegisterInstance<TType, TType>(instance);
        }
        public void RegisterInstance<TBaseType, TType>(TType instance) where TType : TBaseType
        {
            var type = typeof(TBaseType);
            if (_register.ContainsKey(type))
                throw new Exception($"Type of '{type.FullName}' is already registered in DI container.");
            
            _register[type] = new DIEntry(instance);
        }

        public DIEntry RegisterFactory<TType>(Func<TType> factory)
        {
            return RegisterFactory<TType, TType>(factory);
        }
        public DIEntry RegisterFactory<TBaseType, TType>(Func<TType> factory) where TType : TBaseType
        {
            var type = typeof(TBaseType);
            if (_register.ContainsKey(type))
                throw new Exception($"Type of '{type.FullName}' is already registered in DI container.");
            
            var entry = new DIEntry(factory);
            _register[type] = entry;
            return entry;
        }
        
        public T Resolve<T>()
        {
            var type = typeof(T);
            if (_register.TryGetValue(type, out var entry))
            {
                return (T)entry.Resolve();
            }
            
            throw new Exception($"Type of '{type.FullName}' is not registered in DI container.");
        }

        public void Dispose()
        {
            foreach (var entry in _register.Values)
            {
                entry.Dispose();
            }
        }
    }
}