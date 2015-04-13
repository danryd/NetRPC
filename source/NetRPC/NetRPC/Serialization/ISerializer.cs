using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Serialization
{
    public interface ISerializer
    {
        RequestEnvelope DeserializeRequest(string request);
        ResponseEnvelope DeserializeResponse(string request);
        string SerializeRequest(RequestEnvelope request);
        string SerializeResponse(ResponseEnvelope response);

    }
}
