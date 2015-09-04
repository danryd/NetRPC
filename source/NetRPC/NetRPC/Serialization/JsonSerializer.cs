using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fastJSON;

namespace NetRPC.Serialization
{
    public class JsonSerializer : ISerializer
    {
        public JsonSerializer()
        {
            fastJSON.JSON.Parameters = new JSONParameters
                {
                    UseExtensions = false,
                    UseFastGuid = false
                };
        }
        public Message Deserialize(string request)
        {
            
            return fastJSON.JSON.ToObject<Message>(request);
        }

        public Message DeserializeResponse(string request)
        {
            return fastJSON.JSON.ToObject<Message>(request);
        }

        public object DeserializeParameter(Parameter parameter)
        {
            if (parameter.Type == "Void")
                return null;
            var type= Type.GetType(parameter.Type);
            return fastJSON.JSON.ToObject(parameter.Value,type );
        }
        public Parameter SerializeToParameter(object o) {
            if (o == null)
                return null;
            var para = new Parameter();
            para.Value = fastJSON.JSON .ToJSON(o);
            para.Type = o.GetType().FullName + ", " + o.GetType().Assembly.GetName().Name;
            return para;
        }

        public string SerializeRequest(Message request)
        {
            return fastJSON.JSON.ToJSON(request);
        }

        public string Serialize(Message response)
        {
            var json = fastJSON.JSON.ToJSON(response);
            return json;
        }
    }
}
