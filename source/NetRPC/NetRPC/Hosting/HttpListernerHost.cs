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
    public class HttpListenerHost : HostBase
    {
        private HttpListener listener;
        private Thread workerThread;
        private bool isOpen;
        private Uri uri;
        private StreamHandler streamHandler = new StreamHandler();
        public HttpListenerHost(string uri, IServiceContainer container)
            : this(uri, container, AuthenticationSchemes.Anonymous)
        {
        }

        public HttpListenerHost(string uri, IServiceContainer container, AuthenticationSchemes scheme)
            : this(uri, container, scheme, new HttpListener())
        {
        }
        public HttpListenerHost(string uri, IServiceContainer container, AuthenticationSchemes scheme, HttpListener sharedListener)
            : base(container)
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
                    var message = streamHandler.ReadToString(ctx.Request.InputStream);
                    var response = await Process(endpointUri, message);
                    WriteOutput(ctx, response);
                }
                catch (InvalidOperationException ex)
                {
                    ReturnErrorResponse(ctx, ex.Message);
                }
            }
        }

        private void WriteOutput(HttpListenerContext ctx, string response)
        {

            var bytes = Encoding.UTF8.GetBytes(response);
            ctx.Response.ContentLength64 = bytes.Length;
            ctx.Response.ContentType = "json/rpc";
            streamHandler.WriteToStream(bytes, ctx.Response.OutputStream);

            ctx.Response.Close();
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

        public void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                isOpen = false;
                listener.Close();
            }
        }
        protected void OnMessageReceived(MessageReceivedEventArgs eventArgs)
        {
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
