namespace NetRPC
{
    using NetRPC.Invocation;
    using NetRPC.Hosting;
    using NetRPC.Serialization;
    using NetRPC.Transport;
    using System;
    using System.IO;
    using System.Linq;
    public class Pipeline
    {
        private readonly IServiceFactory serviceFactory;
        private Type contract;
        //private StreamHandler streamHandler = new StreamHandler();
        private readonly ISerializer serializer;
        public Pipeline(Type contract, ISerializer serializer, IServiceFactory factory, IServiceInvoker invoker)
        {
            this.serializer = serializer;
            this.serviceFactory = factory;
            this.contract = contract;

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
                Error = context.Error
            };

        }

        private void Serialize(NetRPCContext context)
        {
            context.Response.Result = serializer.SerializeToParameter(context.Result);
            var json = serializer.SerializeResponse(context.Response);
            context.ResponseString = json;

        }

        private IServiceInvoker invoker = new DefaultInvoker();
        private void Dispatch(NetRPCContext context)
        {

            var serviceInstance = serviceFactory.Create();
            try
            {
                var result = invoker.Dispatch(contract, context.Request.Method, serviceInstance, context.Parameters);
                context.Result = result;
            }
            catch (Exception ex)
            {
                context.Error = new Error { Code = 300, Description = ex.Message };
                throw;
            }
            finally
            {
                serviceFactory.Release(serviceInstance);
            }

        }
        private void Deserialize(NetRPCContext context)
        {
            try
            {
                context.Request = serializer.DeserializeRequest(context.RequestString);
                context.Parameters = DeserializeParameters(context);
            }
            catch (Exception ex)
            {
                throw new NetRPCException(300, ex.Message, ex);

            }

        }

        private object[] DeserializeParameters(NetRPCContext context)
        {
            if (context.Request.Parameters == null)
                return null;
            return context.Request.Parameters.Select(p => serializer.DeserializeParameter(p)).ToArray();
        }


    }


}
