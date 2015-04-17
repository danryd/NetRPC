using NetRPC.Transport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace NetRPC.Hosting
{
    public class HostBase : IDisposable
    {
        private readonly IServiceContainer container;
        public HostBase(IServiceContainer container)
        {
            this.container = container;
        }

        protected Task<string> Process(string endpoint, string message)
        {
            return Task.Run<string>(() => container.Handle(endpoint, message));
        }
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing) { }

    }

    public class Host:IDisposable
    {
        private readonly IServiceContainer serviceContainer;
        private readonly ITransport[] transports;
        private Thread[] connections;
        public Host(IServiceContainer serviceContainer, ITransport[] transports)
        {
            this.serviceContainer = serviceContainer;
            this.transports = transports;
            Init();
        }

        private void Init()
        {
            var connectionCount = transports.Length;
            connections = new Thread[connectionCount];
            for (int i = 0; i < connectionCount; i++) {
                connections[i] = new Thread(async () =>
                {
                    await Connect(serviceContainer, transports[i]);
                });
            }
        }

        protected async Task Connect(IServiceContainer container, ITransport transport) {
            var request = await transport.Receive();
            container.Handle("","")
        
        }
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing) {
        
        }

    }
  
}
