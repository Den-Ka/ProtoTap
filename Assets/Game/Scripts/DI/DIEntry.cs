using System;

namespace Etern0nety.DI
{
    public class DIEntry : IDisposable
    {
        private object _instance;
        private readonly Func<object> _factory;
        protected bool _isSingleton = false;

        public DIEntry(object instance)
        {
            _instance = instance;
            AsSingle();
        }
        public DIEntry(Func<object> factory)
        {
            _factory = factory;
        }
        
        public void AsSingle()
        {
            _isSingleton = true;
        }

        public object Resolve()
        {
            if(!_isSingleton || _instance == null) _instance = _factory();
            
            return _instance;
        }

        public void Dispose()
        {
            if(_instance is IDisposable disposable) disposable.Dispose();
        }
    }
}