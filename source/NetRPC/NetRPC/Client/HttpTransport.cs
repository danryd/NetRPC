using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Client
{
    class HttpTransport
    {
        private Uri uri;
        private StreamHandler streamHandler = new StreamHandler();
        public HttpTransport(string uri)
        {
            this.uri = new Uri(uri);

        }

        public string Request(string request)
        {
            var client = WebRequest.Create(uri);
            client.Method = "POST";
            client.ContentType = "application/json";
            streamHandler.WriteToStream(request.ToByteArray(), client.GetRequestStream());
            var resp = client.GetResponse();
            return streamHandler.ReadToString(resp.GetResponseStream());

        }
    }
}
