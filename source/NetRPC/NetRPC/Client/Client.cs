

namespace NetRPC.Client
{
    using NetRPC.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;
    using System.Runtime.Remoting.Proxies;

    /// <summary>
    /// NetRPC client class, implement with service interface
    /// 
    /// </summary>
    /// <typeparam name="T">Service interface</typeparam>
    public class Client<T>
    {
        private readonly ActualProxy<T> actual;
        /// <summary>
        ///  Uses default transport (HTTP) and default serializer (JSON)
        /// </summary>
        /// <param name="uri">Full uri to endpoint (ie. http://example.com/endpoint )</param>
        public Client(string uri)
            : this(new HttpTransport(uri))
        {
        }
        public Client(IClientTransport transport) : this(transport, new JsonSerializer()) { }
        public Client(IClientTransport transport, ISerializer serializer)
        {
            actual = new ActualProxy<T>(transport, serializer);
        }
        public T Proxy()
        {
            return (T)actual.GetTransparentProxy();
        }
        public IDictionary<string, string> RequestHeaderdata { get { return actual.RequestHeaderdata; } }
        public IDictionary<string, string> ResponseHeaderdata { get { return actual.ResponseHeaderdata; } }
    
        /// <summary>
        /// Inner class to reduce external surface area.
        /// </summary>
        /// <typeparam name="T">Service interface</typeparam>
        private class ActualProxy<TInner> : RealProxy
        {
            private ISerializer serializer = new JsonSerializer();
            private IClientTransport transport;
            private Dictionary<string, string> requestHeaderData;
            private Dictionary<string, string> responseHeaderData;
          
            public ActualProxy(IClientTransport transport, ISerializer serializer)
                : base(typeof(T))
            {
                this.serializer = serializer;
                this.transport = transport;
                this.requestHeaderData = new Dictionary<string, string>();
                this.responseHeaderData = new Dictionary<string, string>();
            
            }
            public override IMessage Invoke(IMessage msg)
            {
                var mcm = msg as IMethodCallMessage;
                if (mcm == null)
                    throw new NetRPCException(600, "Proxy call was not of type IMethodCallMessage, instead type was: " + msg.GetType());
                var request = CreateRequest(mcm);
                var response = Process(request);
                var result = Process(response, mcm);
                var returnMessage = new ReturnMessage(result, null, 0, null, mcm);
                return returnMessage;
            }
            public IDictionary<string, string> RequestHeaderdata { get { return requestHeaderData; } }
            public IDictionary<string, string> ResponseHeaderdata { get { return responseHeaderData; } }
          
            private object Process(Message response, IMethodCallMessage mcm)
            {
                var returnType = ((MethodInfo)mcm.MethodBase).ReturnType;
                responseHeaderData = response.Headers;
                if (response.Result != null && returnType != typeof(void))
                {
                  
                    var result = serializer.DeserializeParameter(response.Result);
                   
                    return Convert.ChangeType(result, returnType);
                }
                return null;
            }

            private Message Process(Message request)
            {
                var serializedRequest = serializer.Serialize(request);
                var response = transport.Request(serializedRequest);
                return serializer.Deserialize(response);
            }

            private Message CreateRequest(IMethodCallMessage msg)
            {
                var request = new Message();
                request.Method = msg.MethodName;
                request.SessionId = Guid.NewGuid();
                request.CallId = Guid.NewGuid();
                request.Version = Constants.Version;
                request.Parameters = (msg.InArgs).Select(p => serializer.SerializeToParameter(p)).ToArray();
                request.Headers = requestHeaderData;
                return request;
            }


        }
    }
}