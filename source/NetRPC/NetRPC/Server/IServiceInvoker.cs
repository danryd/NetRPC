using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Server
{
    public interface IServiceInvoker
    {
        object Dispatch(Type service, string action, object instance, object[] parameters);
    }
}
