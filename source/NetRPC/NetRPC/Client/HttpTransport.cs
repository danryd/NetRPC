using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Client
{
    class HttpTransport:IClientTransport
    {
        private Uri uri;
        private readonly StreamHandler streamHandler = new StreamHandler();
        public HttpTransport(string uri)
        {
            this.uri = new Uri(uri);

        }

        public string Request(string request)
        {
            var client = WebRequest.Create(uri);
            client.Method = "POST";
            client.ContentType = Constants.ContentType;//should be depeding on serialization tech.
            streamHandler.WriteToStream(request.ToByteArray(), client.GetRequestStream());
            var resp = client.GetResponse();
            return streamHandler.ReadToString(resp.GetResponseStream());

        }
    }
}
