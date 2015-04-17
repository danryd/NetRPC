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
        public static Request GenerateRequest(string method, object[] parameters = null, Dictionary<string, string> headers = null)
        {
            var requestParameters = parameters == null ? null : parameters.Select(p => ToParameter(p)).ToArray();
            var request = new Request
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
            return serializer.SerializeRequest(request);

        }

        public static string GenerateJsonResponse(string method, object parameter = null, Dictionary<string, string> headers = null, Error error = null)
        {
            return serializer.SerializeResponse(GenerateResponse(method, parameter, headers, error));
        }
        public static Response GenerateResponse(string method, object parameter = null, Dictionary<string, string> headers = null, Error error = null)
        {
            var responseParameter = parameter == null ? null : ToParameter(parameter);
            var response = new Response
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
