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
        Message Deserialize(string message);

        string Serialize(Message response);
        object DeserializeParameter(Parameter parameter);
        Parameter SerializeToParameter(object o);
    }
}
