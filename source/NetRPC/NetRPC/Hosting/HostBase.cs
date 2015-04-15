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
        private readonly IServiceContainer container;
        public HostBase(IServiceContainer container)
        {
            this.container = container;
        }

        protected string Process(string endpoint, string message) {
            return container.Handle(endpoint, message);
        }
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing) { }

    }
}
