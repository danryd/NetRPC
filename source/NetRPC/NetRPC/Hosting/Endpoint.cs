using NetRPC.Invocation;
using NetRPC.Serialization;
using NetRPC.Transport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetRPC.Hosting
{
    public class Endpoint : IEndpoint
    {
        //private readonly Type contract;
        //private readonly IServiceFactory serviceFactory;
        private readonly string endpointName;
        private readonly Pipeline pipeline;
        public Endpoint(string endpointName, Pipeline pipeline)
        {
           
            this.endpointName = endpointName;
            this.pipeline = pipeline;

        }
        public Endpoint(string endpointName, IServiceFactory serviceFactory) : 
            this(endpointName,new Pipeline(new JsonSerializer(), serviceFactory, new DefaultInvoker())) { 
        }
        public string Handle(string request)
        {
            return pipeline.Handle(request);
        }

        public string EndpointName
        {
            get { return endpointName; }
        }
    }
    public interface IEndpoint
    {

        string EndpointName { get; }
        string Handle(string request);
    }
}
