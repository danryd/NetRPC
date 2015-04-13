using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Serialization
{
    public class JsonSerializer : ISerializer
    {

        public RequestEnvelope DeserializeRequest(string request)
        {

            return fastJSON.JSON.ToObject<RequestEnvelope>(request);
        }

        public ResponseEnvelope DeserializeResponse(string request)
        {
            return fastJSON.JSON.ToObject<ResponseEnvelope>(request);
        }


        public string SerializeRequest(RequestEnvelope request)
        {
            return fastJSON.JSON.ToJSON(request);
        }

        public string SerializeResponse(ResponseEnvelope response)
        {
            var json = fastJSON.JSON.ToJSON(response);
            return json;
        }
    }
}
