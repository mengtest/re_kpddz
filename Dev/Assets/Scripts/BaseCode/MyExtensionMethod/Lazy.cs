#region

using System;

#endregion

namespace MyExtensionMethod
{
    public sealed class Lazy<T>
    {
        private readonly Func<T> _function;
        private readonly object _forceLock = new object();
        private T _value;

        public Lazy(Func<T> function)
        {
            _function = function;
        }

        public Lazy(T value)
        {
            _value = value;
        }

        public T Value
        {
            get { return Force(); }
        }

        public bool IsForced { get; private set; }

        public bool IsException
        {
            get { return Exception != null; }
        }

        public Exception Exception { get; private set; }

        public T Force()
        {
            lock (_forceLock) {
                return UnsynchronizedForce();
            }
        }

        public T UnsynchronizedForce()
        {
            if (Exception != null)
                throw Exception;
            if (_function != null && !IsForced) {
                try {
                    _value = _function();
                    IsForced = true;
                } catch (Exception ex) {
                    Exception = ex;
                    throw;
                }
            }
            return _value;
        }
    }
}