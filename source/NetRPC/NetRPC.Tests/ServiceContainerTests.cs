namespace NetRPC.Tests
{
    using NetRPC.Server;
    using Should;
    using Should.Core.Assertions;
    public class ServiceContainerTests
    {
        private IServiceContainer container = new ServiceContainer();
        public void ContainerHandlesRequest()
        {
            container.AddEndpoint(new DummyEndpoint());
            var output = container.Handle("dummy", "anything");
            output.ShouldEqual("handledRequest");
        }
        public void ContainerThrowsExceptionIfEndpointNotFound()
        {
           
                Assert.Throws<EndpointNotFoundException>(()=>container.Handle("notregisterd", ""));
           

        }

    }

    public class DummyEndpoint : IEndpoint
    {
        public string EndpointName
        {
            get { return "dummy"; }
        }

        public string Handle(string request)
        {
            return "handledRequest";
        }
    }
}
