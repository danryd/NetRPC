using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Serialization
{
   public class JsonSerializer:Serializer
    {

        public override RequestEnvelope DeserializeRequest(Byte[] request)
        {
            var json = Encoding.UTF8.GetString(request);
            return fastJSON.JSON.ToObject<RequestEnvelope>(json);
        }

        public override ResponseEnvelope DeserializeResponse(Byte[] request)
        {
            throw new NotImplementedException();
        }

        public override object Parameter(Parameter parameter)
        {
            throw new NotImplementedException();
        }

        public override Parameter Parameter(object o)
        {
            throw new NotImplementedException();
        }

        public override byte[] SerializeRequest(RequestEnvelope request)
        {
            throw new NotImplementedException();
        }

        public override byte[] SerializeReponose(ResponseEnvelope response)
        {
            throw new NotImplementedException();
        }
    }
}
