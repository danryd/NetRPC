using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Server
{
    public class NetRPCContext
    {
        public string RequestString { get; set; }
        public string ResponseString { get; set; }
        public Request Request { get; set; }
        public Response Response { get; set; }
        public object Result { get; set; }
        public Error Error { get; set; }

        public object[] Parameters { get; set; }
    }
}
