using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Hosting
{
    public interface IServiceFactory
    {
        object Create();
        void Release(object o);
    }
}
