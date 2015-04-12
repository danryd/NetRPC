using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC
{
    public class Envelope
    {
        /// <summary>
        /// 0.5
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// The remote method to execute
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// Call identifier, must be unique
        /// </summary>
        public Guid CallId { get; set; }
        /// <summary>
        /// Session identifier, unique per session, used to indicate a context between calls
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// OPTIONAL
        /// Headers
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

    }
    public class RequestEnvelope : Envelope
    {
        /// <summary>
        /// OPTIONAL
        /// Parameters to method. Requires correct ordering
        /// Empty or null if method has no parameters
        /// </summary>
        public Parameter[] Parameters;
    }
    public class ResponseEnvelope : Envelope
    {
        public object Result { get; set; }
        public Error Error { get; set; }
    }
    public class Error
    {
        public int Code { get; set; }
        public string Description { get; set; }
    }
    public class Parameter
    {
        /// <summary>
        /// Parameter type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Serialized instance of type
        /// </summary>
        public string Value { get; set; }
    }
}
