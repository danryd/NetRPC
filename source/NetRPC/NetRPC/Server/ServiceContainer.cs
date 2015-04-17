using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Server
{

    public interface IServiceContainer
    {
        void AddEndpoint(IEndpoint endpoint);
        string Handle(string endpoint, string request);
    }
    public class ServiceContainer : IServiceContainer
    {
        private IDictionary<string, IEndpoint> endpoints = new Dictionary<string, IEndpoint>();
        public void AddEndpoint(IEndpoint endpoint)
        {
            endpoints.Add(endpoint.EndpointName, endpoint);
        }
        public string Handle(string endpointName, string request)
        {
            if (!endpoints.ContainsKey(endpointName))
                throw new EndpointNotFoundException(string.Format("The endpoint ({0}) was not found", endpointName));
;                
            var endpoint = endpoints[endpointName];
            return endpoint.Handle(request);
        }
    }
}
