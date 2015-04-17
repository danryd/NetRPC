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
        Request DeserializeRequest(string request);
        Response DeserializeResponse(string response);
        string SerializeRequest(Request request);
        string SerializeResponse(Response response);
        object DeserializeParameter(Parameter parameter);
        Parameter SerializeToParameter(object o);
    }
}
