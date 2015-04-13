using NetRPC.Transport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetRPC.Hosting
{
    public class Endpoint
    {
        private readonly Type contract;
        private readonly IServiceFactory serviceFactory;
        private readonly string relativeAddress;
        private readonly Pipeline pipeline;
        private readonly ITransport transport;
        public Endpoint(Type contract, ITransport transport, IServiceFactory serviceFactory, string relativeAddress)
        {
            this.contract = contract;
            this.serviceFactory = serviceFactory;
            this.relativeAddress = relativeAddress;
            this.pipeline = new Pipeline(serviceFactory,contract,transport);
            this.transport = transport;
            this.transport.MessageReceiveced += transport_MessageReceiveced;
        }

        void transport_MessageReceiveced(object sender, MessageReceivedEventArgs e)
        {
            var response = pipeline.Handle(e.Message);
            transport.Send(response);
        }

      

    }
}
