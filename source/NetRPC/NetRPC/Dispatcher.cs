namespace NetRPC
{
    using NetRPC.Invocation;
    using NetRPC.Hosting;
    using NetRPC.Serialization;
    using NetRPC.Transport;
    using System;
    using System.IO;
    public class Pipeline
    {
        private readonly IServiceFactory serviceFactory;
        //private Type service;
        //private StreamHandler streamHandler = new StreamHandler();
        private readonly ISerializer serializer;
        public Pipeline(ISerializer serializer, IServiceFactory factory, IServiceInvoker invoker)
        {
            this.serializer = serializer;
            this.serviceFactory = factory;
            //this.service = service;

        }
        public string Handle(string request)
        {
            var context = CreateContext(request);
            Deserialize(context);
            Dispatch(context);
            CreateResponse(context);
            Serialize(context);
            return context.ResponseString;

        }

        private static NetRPCContext CreateContext(string request)
        {
            var context = new NetRPCContext();
            context.RequestString = request;
            return context;
        }
              

        private void CreateResponse(NetRPCContext context)
        {
            context.Response = new Response
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
            context.ResponseString = json;

        }

        private IServiceInvoker dispatcher = new DefaultInvoker();
        private void Dispatch(NetRPCContext context)
        {
            //var serviceInstance = serviceFactory.Create();
            //try
            //{
            //    var result = dispatcher.Dispatch(service, context.Request.Method, serviceInstance, context.Request.Parameters);
            //    context.Result = result;
            //}
            //catch (Exception ex)
            //{
            //    context.Error = new Error { Code = 666, Description = ex.Message };
            //}
            //finally
            //{
            //    serviceFactory.Release(serviceInstance);
            //}

        }
           private void Deserialize(NetRPCContext context)
        {
            context.Request = serializer.DeserializeRequest(context.RequestString);
        }


    }


}
