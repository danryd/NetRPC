using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace NetRPC.Server
{
    public class HostBase : IDisposable
    {
        private readonly IServiceContainer container;
        public HostBase(IServiceContainer container)
        {
            this.container = container;
        }

        protected async Task<string> Process(string endpoint, string message) {
            var response = await Task.Run<string>(()=> container.Handle(endpoint, message));
            return response;
        }
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing) { }

    }
}
