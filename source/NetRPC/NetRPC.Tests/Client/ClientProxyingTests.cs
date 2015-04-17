namespace NetRPC.Tests.Client
{
    using NetRPC.Client;
    using Should;
    public class ClientProxyingTests
    {

        protected void CanCreateClientOfT()
        {
            var client = new Client<ProxyThis>("null");
           // var str = client.Proxy().Method("what");
            //str.ShouldEqual(4);
        }
    }

    public interface ProxyThis
    {
         int Method(string data);
    }
}
