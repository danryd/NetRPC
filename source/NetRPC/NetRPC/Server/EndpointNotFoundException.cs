namespace NetRPC.Server
{
    using System;

    public class EndpointNotFoundException : Exception
    {
        public EndpointNotFoundException() { }
        public EndpointNotFoundException(string message) : base(message) { }
        public EndpointNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected EndpointNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

}
