using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NetRPC.SystemWebHost
{
    public class NetRPCHandler:IHttpAsyncHandler
    {
        Host host;
        static NetRPCHandler() { 
        
        
        }
        public bool IsReusable
        {
            get { return true; }
        }

     

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            throw new NotImplementedException();
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            throw new NotImplementedException();
        }


        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
