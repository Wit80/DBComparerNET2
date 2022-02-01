using System;
using System.Runtime.Serialization;

namespace DBComparerLibrary
{
    [System.Serializable]
    public class ComparerException : Exception
    {
        private object _value;
        public ComparerException() { }
        public ComparerException(string message) : base(message) { }

        public ComparerException(string message, Exception innerException) : base(message, innerException) { }

        protected ComparerException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public ComparerException(string message, object value) : this(message)
        {
            this._value = value;

        }
        public object GetAddInfo() => _value;
    }
}
