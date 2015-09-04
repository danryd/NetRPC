using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRPC.Serialization;

namespace NetRPC.Server.Stages
{
    class SerializationStage : IPipelineStage
    {
        private ISerializer serializer = new JsonSerializer();
        public void Incoming(NetRPCContext context)
        {
            context.Response.Error = context.Error;
            context.Response.Result = serializer.SerializeToParameter(context.Result);
            var serializedResponse = serializer.Serialize(context.Response);
            context.ResponseString = serializedResponse;
        }

        public void Outgoing(NetRPCContext context)
        {
            try
            {
                context.Request = serializer.Deserialize(context.RequestString);
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
