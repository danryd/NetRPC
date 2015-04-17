using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Transport
{
    public interface ITransport
    {
        Task<string> Receive();
        Task Send(string message);

    }
   
}
