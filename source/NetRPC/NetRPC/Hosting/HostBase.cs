using NetRPC.Transport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace NetRPC.Hosting
{
    public class HostBase : IDisposable
    {
        private readonly Dictionary<string, Endpoint> endpoints;
        public  HostBase()
        {
             endpoints = new Dictionary<string, Endpoint>();
        }

        public void AddEndpoint<TContract>(ITransport transport, IServiceFactory serviceFactory, string relativeAddress) where TContract : class
        {
            var endpoint = new Endpoint(typeof(TContract), transport,serviceFactory, relativeAddress);
            endpoints.Add(relativeAddress, endpoint);
        }
        protected Endpoint FindEndpoint(string endpointUri)
        {
            if (endpoints.ContainsKey(endpointUri))
                return endpoints[endpointUri];
            throw new InvalidOperationException("Endpoint not found");
        }
       
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing) { }

    }
}
