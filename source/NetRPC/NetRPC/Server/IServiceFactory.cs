using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Server
{
    public interface IServiceFactory
    {
        object Create(OperationContext operationContext);
        void Release(object o);
    }
}
