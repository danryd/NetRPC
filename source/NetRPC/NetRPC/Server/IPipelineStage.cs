namespace NetRPC.Server 
{
    interface IPipelineStage
    {
        void Incoming(NetRPCContext context);
        void Outgoing(NetRPCContext context);
    }
}
