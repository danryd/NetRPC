using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Serialization
{
    public class JsonSerializer : ISerializer
    {

        public Request DeserializeRequest(string request)
        {

            return fastJSON.JSON.ToObject<Request>(request);
        }

        public Response DeserializeResponse(string request)
        {
            return fastJSON.JSON.ToObject<Response>(request);
        }

        public object DeserializeParameter(Parameter parameter)
        {
            if (parameter.Type == "Void")
                return null;
            return fastJSON.JSON.ToObject(parameter.Value, Type.GetType(parameter.Type));
        }
        public Parameter SerializeToParameter(object o) {
            var para = new Parameter();
            para.Value = fastJSON.JSON.ToJSON(o);
            para.Type = o.GetType().Name;
            return para;
        }

        public string SerializeRequest(Request request)
        {
            return fastJSON.JSON.ToJSON(request);
        }

        public string SerializeResponse(Response response)
        {
            var json = fastJSON.JSON.ToJSON(response);
            return json;
        }
    }
}
