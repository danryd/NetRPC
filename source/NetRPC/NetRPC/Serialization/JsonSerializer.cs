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
            var json = Encoding.UTF8.GetString(request);
            return fastJSON.JSON.ToObject<ResponseEnvelope>(json);
        }

      
        public override byte[] SerializeRequest(RequestEnvelope request)
        {
            var json = fastJSON.JSON.ToJSON(request);
            return json.ToByteArray();
        }

        public override byte[] SerializeResponse(ResponseEnvelope response)
        {
            var json = fastJSON.JSON.ToJSON(response);
            return json.ToByteArray();
        }
    }
}
