
using System;
using System.Runtime.Serialization;

namespace NetRPC
{
    [Serializable]
    public class NetRPCException : Exception
    {
        private int code;
        public NetRPCException(int code)
            : this(code, null)
        {
        }

        public NetRPCException(int code, string message)
            : this(code, message, null)
        {
        }

        public NetRPCException(int code, string message, Exception inner)
            : base(message, inner)
        {
            this.code = code;
        }

        protected NetRPCException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }

    public class ErrorCodes
    {
        public int SerializationError { get { return 100; } }
        public int ServiceLookupError { get { return 200; } }
        public int ServiceValidationError { get { return 250; } }
        public int ServiceActivationError { get { return 300; } }
        public int ServiceInvocationError { get { return 350; } }
    }
}
