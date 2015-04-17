using NetRPC.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Client
{
    public class Client<T> : RealProxy
    {

        private ISerializer serializer = new JsonSerializer();
        private HttpTransport transport;
        private string uri;

        public Client(string uri)
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
                throw new NetRPCException(666);
            var request = CreateRequest(msg);
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
            var jsonRequest = serializer.SerializeRequest(request);
            var response = transport.Request(jsonRequest);
            return serializer.DeserializeResponse(response);

        }

        private Request CreateRequest(System.Runtime.Remoting.Messaging.IMessage msg)
        {
            var request = new Request();
            request.Method = (string)msg.Properties["__MethodName"];
            request.SessionId = Guid.NewGuid();
            request.CallId = Guid.NewGuid();
            request.Version = "0.5";
            request.Parameters = ((object[])msg.Properties["__Args"]).Select(p => serializer.SerializeToParameter(p)).ToArray();
            return request;
        }
        public T Proxy()
        {
            return (T)this.GetTransparentProxy();
        }
    }
}