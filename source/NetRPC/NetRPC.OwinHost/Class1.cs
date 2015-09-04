using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;

namespace NetRPC.OwinHost
{
    public static class OwinExtensions
    {
        public static IAppBuilder AddNetRPC(this IAppBuilder app, NetRPCOptions options)
        {
            return app;
        }
    }

    public class NetRPCOptions
    {
    }
}
