using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Transport
{
    public interface ITransport
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceiveced;
        void Send(string message);

    }
    public class MessageReceivedEventArgs : EventArgs
    {
        public string Message { get; set; }

        public string Endpoint { get; set; }
    }
}
