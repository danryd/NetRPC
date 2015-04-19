namespace NetRPC.Tests.Client
{
    using NetRPC.Client;
    using NetRPC.Serialization;
    using NSubstitute;
    using Should;
    public class ClientProxyingTests
    {
        private ISerializer serializer = new JsonSerializer();
       

        public void CanSendMessage() {
            IClientTransport trsp = Substitute.For<IClientTransport>();
           
            trsp.Request(Arg.Any<string>()).Returns(MessageHelper.GenerateJsonResponse("Method",4));
            var proxy = new Client<ProxyThis>(trsp);
            var result = proxy.Proxy().Method("asd");
            result.ShouldEqual(4);
        }
    }


    public interface ProxyThis
    {
         int Method(string data);
    }
}
