namespace NetRPC.Tests
{
    using NetRPC.Client;
    using NetRPC.Server;
    using System;
    using Should;
    using NetRPC.SelfHost;
    public class HeaderDataTests
    {
        public void CanAttachHeaderdataToServer()
        {

            string uri = "http://localhost:18081/";
            var container = new ServiceContainer();
            container.AddEndpoint(new Endpoint("Test", typeof(ITest), new DelegateServiceFactory(c => new HeaderdataTestService(c), service => { })));
            using (var server = new Host(uri, container))
            {
                var proxy = new Client<ITest>(uri + "Test");
                proxy.RequestHeaderdata.Add("akey", "avalue");
                proxy.Proxy().VoidNoParam();
                //service.AValue.ShouldEqual("avalue");
            }
        }
        public void CanAttachHeaderdataToReply()
        {

            string uri = "http://localhost:18082/";
            var container = new ServiceContainer();
            container.AddEndpoint(new Endpoint("Test", typeof(ITest), new DelegateServiceFactory(c => new HeaderdataTestService(c), service => {  })));
            using (var server = new Host(uri, container))
            {
                var proxy = new Client<ITest>(uri + "Test");
                proxy.Proxy().VoidStringParam("avalue");
                var response = proxy.ResponseHeaderdata["akey"];
                response.ShouldEqual("avalue");
            }
        }
    }

    public class HeaderdataTestService : ITest
    {
        private OperationContext ctx;
        public HeaderdataTestService(OperationContext ctx)
        {
            this.ctx = ctx;
        }
        public string AValue { get; set; }
        public void VoidNoParam()
        {

            AValue = ctx.IncomingHeaderdata["akey"];
            AValue.ShouldEqual("avalue");
        }

        public void VoidStringParam(string str)
        {
             ctx.OutgoingHeaderdata.Add("akey", str);
        }

        public string StringNoParam()
        {
            throw new NotImplementedException();
        }

        public string Echo(string str)
        {
            throw new NotImplementedException();
        }
    }
}
