namespace NetRPC
{
    using NetRPC.Dispatching;
    using NetRPC.Hosting;
    using NetRPC.Serialization;
    using NetRPC.Transport;
    using System;
    using System.IO;
    public class Pipeline
    {
        IServiceFactory serviceFactory;
        private Type service;
        private StreamHandler streamHandler = new StreamHandler();
        public Pipeline(IServiceFactory serviceFactory, Type service, ITransport transport)
        {
            // TODO: Complete member initialization
            this.serviceFactory = serviceFactory;
            this.service = service;

        }
        public string Handle(string inputStream)
        {
            var context = new NetRPCContext();
            context.InputStream = inputStream;
            
            Handle(context);
            return context.OutputStream;

        }
        protected void Handle(NetRPCContext context)
        {
            Deserialize(context);
            Dispatch(context);
            CreateResponse(context);
            Serialize(context);
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
            var json = serializer.SerializeResponse(context.Response);
            context.OutputStream = json;

        }

        private Dispatcher dispatcher = new DefaultDispatcher();
        private void Dispatch(NetRPCContext context)
        {
            var serviceInstance = serviceFactory.Create();
            try
            {
                var result = dispatcher.Dispatch(service, context.Request.Method, serviceInstance, context.Request.Parameters);
                context.Result = result;
            }
            catch (Exception ex)
            {
                context.Error = new Error { Code = 666, Description = ex.Message };
            }
            finally
            {
                serviceFactory.Release(serviceInstance);
            }

        }
        private ISerializer serializer = new JsonSerializer();
        private void Deserialize(NetRPCContext context)
        {
            context.Request = serializer.DeserializeRequest(context.InputStream);
        }


    }


}
