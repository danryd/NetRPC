using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace NetRPC.Hosting
{
    public abstract class HostBase : IDisposable
    {
        private readonly Dictionary<string, Endpoint> endpoints;
        protected HostBase()
        {
             endpoints = new Dictionary<string, Endpoint>();
        }

       

        public void AddEndpoint<TContract>(Func<object> factory, string relativeAddress) where TContract : class
        {
            var endpoint = new Endpoint(typeof(TContract), factory, relativeAddress);
            endpoints.Add(relativeAddress, endpoint);
        }
        protected Endpoint FindEndpoint(string endpointUri)
        {
            if (endpoints.ContainsKey(endpointUri))
                return endpoints[endpointUri];
            throw new InvalidOperationException("Endpoint not found");
        }
        protected void OnReceive(string endpointAddress, Stream payload)
        {
            var endpoint = endpoints[endpointAddress];
            var instance = endpoint.CreateInstance();
        }
        public void Dispose()
        {
            Dispose(true);
        }

        protected abstract void Dispose(bool isDisposing);

    }
}
