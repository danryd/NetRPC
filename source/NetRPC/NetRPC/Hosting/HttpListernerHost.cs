using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetRPC.Hosting
{
    public class HttpListenerHost : HostBase
    {
        private HttpListener listener;
        private Thread workerThread;
        private bool isOpen;
        private Uri uri;
        public HttpListenerHost(string uri)
            : this(uri, AuthenticationSchemes.Anonymous)
        {
        }

        public HttpListenerHost(string uri, AuthenticationSchemes scheme)
        {
            this.uri = new Uri(uri);
            listener = new HttpListener();
            listener.Prefixes.Add(uri);
            listener.AuthenticationSchemes = scheme;

            listener.Start();
            isOpen = true;
            workerThread = new Thread(new ParameterizedThreadStart(Listen));
            workerThread.Start();
        }

         private async void Listen(object o)
        {
            while (isOpen)
            {
                var ctx = await listener.GetContextAsync();
                if (ctx.Request.HttpMethod != "POST")
                {
                    ReturnErrorResponse(ctx, "POST is the only verb that is understood");
                }
                try
                {
                    var endpointUri = uri.MakeRelativeUri(ctx.Request.Url).ToString();
                    var endpoint = FindEndpoint(endpointUri);
                    endpoint.Handle(ctx.Request.InputStream, ctx.Response.OutputStream);
                }
                catch (InvalidOperationException ex)
                {
                    ReturnErrorResponse(ctx, ex.Message);
                }





            }
        }



        private static void ReturnErrorResponse(HttpListenerContext ctx, string message)
        {
            ctx.Response.StatusCode = 500;
            ctx.Response.StatusDescription = message;
            ctx.Response.ContentType = "text";
            var respsonse = Encoding.UTF8.GetBytes("500 - internal server error");
            ctx.Response.OutputStream.Write(respsonse, 0, respsonse.Length);
            ctx.Response.Close();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                isOpen = false;
                listener.Close();
            }
        }

    }
}
