namespace NetRPC.Server
{
    using NetRPC.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class Pipeline
    {
        private readonly IServiceFactory serviceFactory;
        private Type contract;
        private readonly ISerializer serializer;
        public Pipeline(Type contract, ISerializer serializer, IServiceFactory factory, IServiceInvoker invoker)
        {
            this.serializer = serializer;
            this.serviceFactory = factory;
            this.contract = contract;

        }
        public string Handle(string request)
        {
            try
            {
                var context = CreateContext(request);
                Deserialize(context);
                Dispatch(context);
                Serialize(context);
                return context.ResponseString;
            }
            finally
            {

            }


        }

      

        private static NetRPCContext CreateContext(string request)
        {
            var context = new NetRPCContext();
            context.RequestString = request;
            return context;
        }


        

        private void Serialize(NetRPCContext context)
        {
            context.Response.Error = context.Error;
            context.Response.Result = serializer.SerializeToParameter(context.Result);
            var serializedResponse = serializer.SerializeResponse(context.Response);
            context.ResponseString = serializedResponse;

        }

        private IServiceInvoker invoker = new DefaultInvoker();
        private void Dispatch(NetRPCContext context)
        {

            var serviceInstance = serviceFactory.Create(new OperationContext(context));
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
                InitializeResponse(context);
            }
            catch (Exception ex)
            {
                throw new NetRPCException(300, ex.Message, ex);

            }

        }

        private void InitializeResponse(NetRPCContext context)
        {
            context.Response = new Response
            {
                CallId = context.Request.CallId,
                SessionId = context.Request.SessionId,
                Version = Constants.Version,
                Method = context.Request.Method,
                Headers = new Dictionary<string,string>()
            };
        }

        private object[] DeserializeParameters(NetRPCContext context)
        {
            if (context.Request.Parameters == null)
                return null;
            return context.Request.Parameters.Select(p => serializer.DeserializeParameter(p)).ToArray();
        }


    }


}
