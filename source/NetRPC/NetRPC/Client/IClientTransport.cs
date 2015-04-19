using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Client
{
    public interface IClientTransport
    {

        /// <summary>
        /// Send request and handle response
        /// </summary>
        /// <param name="request"> Serialized request</param>
        /// <returns>Serialized response</returns>
        string Request(string request);
    }
}
