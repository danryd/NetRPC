using NetRPC.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetRPC.SelfHost
{
    public class Host : HostBase
    {
        private HttpListener listener;
        private Thread workerThread;
        private Task workerTask;
        private bool isOpen;
        private Uri uri;
        private StreamHandler streamHandler = new StreamHandler();
        public Host(string uri, IServiceContainer container)
            : this(uri, container, AuthenticationSchemes.Anonymous)
        {
        }

        public Host(string uri, IServiceContainer container, AuthenticationSchemes scheme)
            : this(uri, container, scheme, new HttpListener())
        {
        }
        public Host(string uri, IServiceContainer container, AuthenticationSchemes scheme, HttpListener sharedListener)
            : base(container)
        {
            this.uri = new Uri(uri);
            listener = sharedListener;
            listener.Prefixes.Add(uri);
            listener.AuthenticationSchemes = scheme;

            listener.Start();
            isOpen = true;
            workerTask = Task.Run(async () => await Listen());
            //workerThread = new Thread(new ParameterizedThreadStart(Listen));
            //workerThread.Start();
        }
        private async Task Listen()
        {
            while (isOpen)
            {
                await listener.GetContextAsync().ContinueWith(async t =>
                {
                    var ctx = await t;
                    if (ctx.Request.HttpMethod != "POST")
                    {
                        ReturnErrorResponse(ctx, "POST is the only verb that is understood");
                    }
                    try
                    {
                        await Task.Run(() =>
                        {
                            var endpointUri = uri.MakeRelativeUri(ctx.Request.Url).ToString();
                            var message = streamHandler.ReadToString(ctx.Request.InputStream);
                            var response = ProcessMessage(endpointUri, message);
                            Reply(ctx, response);

                        });
                    }
                    catch (Exception ex)//Must catch all to return error
                    {
                        ReturnErrorResponse(ctx, ex.Message);
                    }
                });

            }
        }

        private void Reply(HttpListenerContext ctx, string response)
        {

            var bytes = Encoding.UTF8.GetBytes(response);
            ctx.Response.ContentLength64 = bytes.Length;
            ctx.Response.ContentType = Constants.ContentType;//should depend on serialization tech
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

    }
}
