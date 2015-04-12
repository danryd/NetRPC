using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC
{
    public class NetRPCContext
    {
        public Stream InputStream { get; set; }
        public Stream OutputStream { get; set; }
        public RequestEnvelope Request { get; set; }
        public ResponseEnvelope Response { get; set; }
        public object Result { get; set; }

        public Error Error { get; set; }
    }
}
