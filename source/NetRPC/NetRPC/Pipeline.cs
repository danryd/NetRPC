using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC
{
    using NetRPC.Dispatching;
    using NetRPC.Serialization;
    using System.IO;
    public class Pipeline
    {
        private Func<object> factory;
        private Type service;
        private StreamHandler streamHandler = new StreamHandler();
        public Pipeline(Func<object> factory, Type service)
        {
            // TODO: Complete member initialization
            this.factory = factory;
            this.service = service;

        }
        public void Handle(Stream inputStream, Stream outputStream)
        {
            var context = new NetRPCContext();
            context.InputStream = inputStream;
            context.OutputStream = outputStream;
            Handle(context);

        }
        protected void Handle(NetRPCContext context)
        {
            Deserialize(context);
            Dispatch(context);
            CreateResponse(context);
            Serialize(context);
            Reply(context);
        }

        private void Reply(NetRPCContext context)
        {
            context.OutputStream.Close();
        }

        private void CreateResponse(NetRPCContext context)
        {
            context.Response = new ResponseEnvelope
            {
                CallId = context.Request.CallId,
                SessionId = context.Request.SessionId,
                Version = "0.5",
                Method = context.Request.Method,
                Result = context.Result,
                Error = context.Error
            };

        }

        private void Serialize(NetRPCContext context)
        {
            var bytes = serializer.SerializeResponse(context.Response);
            streamHandler.WriteToStream(bytes, context.OutputStream);
            
        }

        private Dispatcher dispatcher = new DefaultDispatcher();
        private void Dispatch(NetRPCContext context)
        {
            try
            {
                var result = dispatcher.Dispatch(service, context.Request.Method, factory(), context.Request.Parameters);
                context.Result = result;
            }
            catch (Exception ex)
            {
                context.Error = new Error { Code = 666, Description = ex.Message };
            }

        }
        private Serializer serializer = new JsonSerializer();
        private void Deserialize(NetRPCContext context)
        {
            var bytes = streamHandler.ReadAllBytes(context.InputStream);
            context.Request = serializer.DeserializeRequest(bytes);
        }
       

    }

    public class SerializationHandler : IHandler
    {
        private Serializer serializer = new JsonSerializer();
        public SerializationHandler()
        {

        }
        public void Handle(NetRPCContext context, IHandler next)
        {
            SetRequest(context);
            next.Handle(context, null);
            SerializeAndWriteResponse(context);
        }

        private void SerializeAndWriteResponse(NetRPCContext context)
        {
            var bytes = serializer.SerializeResponse(context.Response);
            context.OutputStream.Write(bytes, 0, bytes.Length);
        }

        private void SetRequest(NetRPCContext context)
        {
            var rqBytes = new byte[context.InputStream.Length];
            context.InputStream.Read(rqBytes, 0, (int)context.InputStream.Length);
            context.Request = serializer.DeserializeRequest(rqBytes);
        }
    }

}
