using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Server
{
    public class DefaultInvoker:IServiceInvoker
    {
       public object Dispatch(Type service, string action, object instance, object[] parameters)
        {
            var method = service.GetMethod(action);
            return method.Invoke(instance, parameters);
        }
    }
}
