using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC
{
    public interface IHandler
    {
        void Handle(NetRPCContext context, IHandler next);
    }
}
