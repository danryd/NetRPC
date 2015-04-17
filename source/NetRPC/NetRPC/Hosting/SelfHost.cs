namespace NetRPC.Hosting
{
    using NetRPC.Transport;

    public class SelfHost : Host
    {
        private IServiceContainer cnt;
        private ITransport[] transport;

        public SelfHost(IServiceContainer cnt, ITransport[] transport)
            : base(cnt, transport)
        {
        }
    }
}
