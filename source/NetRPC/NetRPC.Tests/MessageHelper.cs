using NetRPC.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Tests
{
    class MessageHelper
    {
        private static ISerializer serializer = new JsonSerializer();
        private static Parameter ToParameter(object o)
        {
            return serializer.SerializeToParameter(o);
        }
        public static Message GenerateRequest(string method, object[] parameters = null, Dictionary<string, string> headers = null)
        {
            var requestParameters = parameters == null ? null : parameters.Select(p => ToParameter(p)).ToArray();
            var request = new Message
           {
               CallId = Guid.NewGuid(),
               SessionId = Guid.NewGuid(),
               Headers = headers,
               Method = method,
               Version = Constants.Version,
               Parameters = requestParameters
           };
            return request;
        }
        public static string GenerateJsonRequest(string method, object[] parameters = null, Dictionary<string, string> headers = null)
        {
            var request = GenerateRequest(method, parameters, headers);
            return serializer.Serialize(request);

        }

        public static string GenerateJsonResponse(string method, object parameter = null, Dictionary<string, string> headers = null, Error error = null)
        {
            return serializer.Serialize(GenerateResponse(method, parameter, headers, error));
        }
        public static Message GenerateResponse(string method, object parameter = null, Dictionary<string, string> headers = null, Error error = null)
        {
            var responseParameter = parameter == null ? null : ToParameter(parameter);
            var response = new Message
            {
                CallId = Guid.NewGuid(),
                SessionId = Guid.NewGuid(),
                Headers = headers,
                Method = method,
                Version = Constants.Version,
                Result = responseParameter,
                Error = error
            };
            return response;
        }
    }
}
