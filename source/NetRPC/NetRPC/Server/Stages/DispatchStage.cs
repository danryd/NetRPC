using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Server.Stages
{
    class DispatchStage:IPipelineStage
    {
        public void Incoming(NetRPCContext context)
        {
            throw new NotImplementedException();
        }

        public void Outgoing(NetRPCContext context)
        {
            throw new NotImplementedException();
        }
    }
}
