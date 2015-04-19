using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Server
{
    class OperationContextManager
    {
        [ThreadStatic]
        private static NetRPCContext currentContext;

        public static NetRPCContext CurrentContext
        {
            get { return currentContext; }
            set { currentContext = value; }
        }
    }
    public class OperationContext
    {
        private NetRPCContext current;
        private OperationContext(NetRPCContext ctx)
        {
            current = ctx;
        }
        public Dictionary<string, string> IncomingHeaderdata { get { return current.Request.Headers; } }
        public Dictionary<string, string> OutgoingHeaderdata { get { return current.Response.Headers; } }
        public static OperationContext Current
        {
            get
            {
                return new OperationContext(OperationContextManager.CurrentContext);
            }
        }
    }
}

