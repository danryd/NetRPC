namespace NetRPC.Tests
{
    using NetRPC.Client;
    using NetRPC.Server;
    using System;
    using Should;
    public class HeaderDataTests
    {
        public void CanAttachHeaderdataToServer()
        {

            string uri = "http://localhost:18081/";
            var service = new HeaderdataTestService();
            var container = new ServiceContainer();
            container.AddEndpoint(new Endpoint("Test", typeof(ITest), new DelegateServiceFactory(() => service, _ => { })));
            using (var server = new HttpListenerHost(uri, container))
            {
                var proxy = new Client<ITest>(uri+"Test");
                proxy.Headerdata.Add("akey", "avalue");
                proxy.Proxy().VoidNoParam();
                service.AValue.ShouldEqual("avalue");
            }
        }
    }

    public class HeaderdataTestService : ITest
    {
        public string AValue { get; set; }
        public void VoidNoParam()
        {
            AValue = OperationContext.Current.IncomingHeaderdata["akey"];
        }

        public void VoidStringParam(string str)
        {
            throw new NotImplementedException();
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
