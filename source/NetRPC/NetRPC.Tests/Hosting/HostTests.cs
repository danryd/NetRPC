namespace NetRPC.Tests.Hosting
{
    using NetRPC.Hosting;
    using NetRPC.Transport;
    using Should;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class HostTests
    {
        MockTrsp trsp = new MockTrsp();
        IServiceContainer cnt = new ServiceContainer();
        public void CanStartSelfHost()
        {
            var msg = "test";
            var invoked = false;
            cnt.AddEndpoint(new MockEndpoint(str =>
            {
                str.ShouldEqual(msg);
                invoked = true;
            }));
            using (var host = new Host(cnt, new[] { trsp }))
            {
                trsp.CallReceive(msg);



            }
            invoked.ShouldBeTrue();
        }


    }

    public class MockEndpoint : IEndpoint
    {
        private Action<string> assert;
        public MockEndpoint(Action<string> assert)
        {
            this.assert = assert;
        }
        public string EndpointName
        {
            get { return "Mock"; }
        }

        public string Handle(string request)
        {
            assert(request);
            return "ok";
        }
    }
    public class MockTrsp : ITransport
    {
        private AutoResetEvent are = new AutoResetEvent(false);

        public MockTrsp()
        {

        }

        public void CallReceive(string str)
        {
            msg = str;
            are.Set();
        }
        private string msg;
        public async Task<string> Receive()
        {

            are.WaitOne();
            return msg;

        }

        public async Task Send(string message)
        {
             await Task.Run(() => Thread.Sleep(10));
        }
    }
}