using NetRPC.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetRPC.Hosting
{
    public class HttpListenerTransport :ITransport
    {
        private HttpListener listener;
        private Thread workerThread;
        private bool isOpen;
        private Uri uri;
        private StreamHandler streamHandler = new StreamHandler();
        public HttpListenerTransport(string uri)
            : this(uri, AuthenticationSchemes.Anonymous)
        {
        }

        public HttpListenerTransport(string uri, AuthenticationSchemes scheme):this(uri,scheme,new HttpListener())
        {
        }
        public HttpListenerTransport(string uri, AuthenticationSchemes scheme, HttpListener sharedListener)
        {
            this.uri = new Uri(uri);
            listener = sharedListener;
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
                    var message = streamHandler.ReadAllBytes(ctx.Request.InputStream);

                    OnMessageReceived(new MessageReceivedEventArgs
                    {
                        Message = "",
                        Endpoint = endpointUri
                    });
                    //var endpoint = FindEndpoint(endpointUri);
                    //endpoint.Handle(ctx.Request.InputStream, ctx.Response.OutputStream);
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

        public  void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                isOpen = false;
                listener.Close();
            }
        }
        protected void OnMessageReceived(MessageReceivedEventArgs eventArgs) {
            if (MessageReceiveced != null)
                MessageReceiveced(this, eventArgs);
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceiveced;

        public void Send(string message)
        {
            throw new NotImplementedException();
        }
    }
}
