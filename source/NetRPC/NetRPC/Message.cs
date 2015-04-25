namespace NetRPC
{
    using System;
    using System.Collections.Generic;

    public class Message
    {
        /// <summary>
        /// Current message version
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
    public class Request : Message
    {

       
        /// <summary>
        /// OPTIONAL
        /// Parameters to method. Requires correct ordering
        /// Empty or null if method has no parameters
        /// </summary>
        public Parameter[] Parameters;
    }
    public class Response : Message
    {
        /// <summary>
        /// The output of the service
        /// </summary>
        public Parameter Result { get; set; }
        /// <summary>
        /// If error occures the error property will be set, otherwise it must be null
        /// </summary>
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
