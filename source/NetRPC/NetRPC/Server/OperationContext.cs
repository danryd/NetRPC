using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Server
{

    public class OperationContext
    {
        private NetRPCContext current;
        public OperationContext(NetRPCContext ctx)
        {
            current = ctx;
        }
        public Dictionary<string, string> IncomingHeaderdata { get { return current.Request.Headers; } }
        public Dictionary<string, string> OutgoingHeaderdata { get { return current.Response.Headers; } }

    }
}

