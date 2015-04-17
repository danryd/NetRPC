

namespace NetRPC.Client
{
    using NetRPC.Serialization;
    using System;
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
        private ActualProxy<T> actual;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri">Full uri to endpoint (ie. http://example.com/endpoint )</param>
        public Client(string uri)
           {
            // TODO: Complete member initialization
            actual = new ActualProxy<T>(uri);
        }
        public T Proxy()
        {
            return (T)actual.GetTransparentProxy();
        }
        /// <summary>
        /// Inner class to reduce external surface area.
        /// </summary>
        /// <typeparam name="T">Service interface</typeparam>
        private class ActualProxy<T> : RealProxy
        {
            private ISerializer serializer = new JsonSerializer();
            private IClientTransport transport;
            private string uri;


            public ActualProxy(string uri)
                : base(typeof(T))
            {
                // TODO: Complete member initialization
                this.uri = uri;
                transport = new HttpTransport(uri);
            }
            public override IMessage Invoke(IMessage msg)
            {
                var mcm = msg as IMethodCallMessage;
                if (mcm == null)
                    throw new NetRPCException(600, "Proxy call was not of type IMethodCallMessage, instead type was: "+msg.GetType());
                var request = CreateRequest(mcm);
                var response = Process(request);
                var result = Process(response, mcm);
                var returnMessage = new ReturnMessage(result, null, 0, null, mcm);
                return returnMessage;
            }

            private object Process(Response response, IMethodCallMessage mcm)
            {
                var returnType = ((MethodInfo)mcm.MethodBase).ReturnType;
                if (response.Result != null && returnType != typeof(void))
                {
                    var result = serializer.DeserializeParameter(response.Result);
                    return Convert.ChangeType(result, returnType);
                }
                return null;
            }

            private Response Process(Request request)
            {
                var serializedRequest = serializer.SerializeRequest(request);
                var response = transport.Request(serializedRequest);
                return serializer.DeserializeResponse(response);
            }

            private Request CreateRequest(IMethodCallMessage msg)
            {
                var request = new Request();
                request.Method = msg.MethodName;
                request.SessionId = Guid.NewGuid();
                request.CallId = Guid.NewGuid();
                request.Version = Constants.Version;
                request.Parameters = (msg.InArgs).Select(p => serializer.SerializeToParameter(p)).ToArray();
                return request;
            }
           

        }
    }
}