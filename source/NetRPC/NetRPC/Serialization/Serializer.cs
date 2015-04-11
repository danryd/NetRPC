using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Serialization
{
    public abstract class Serializer
    {
        public abstract RequestEnvelope DeserializeRequest(Byte[] request);
        public abstract ResponseEnvelope DeserializeResponse(Byte[] request);
        public abstract byte[] SerializeRequest(RequestEnvelope request);
        public abstract byte[] SerializeReponose(ResponseEnvelope response);
        public abstract object Parameter(Parameter parameter);
        public abstract Parameter Parameter(object o);
    }
}
