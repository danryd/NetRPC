
namespace NetRPC.Tests
{
    using NetRPC.Client;
    using NetRPC.Server;
    using System;
    using System.Threading;
    using NetRPC.SelfHost;
    public class MultiClientTests
    {
        
        public void FourClientsOneServer() {
            int threadCount = 4;
            int reps = 4;
            string uri = "http://localhost:18080/";
            var threads = new Thread[threadCount];
            var container = new ServiceContainer();
            container.AddEndpoint(new Endpoint("Test", typeof(ITest), new DelegateServiceFactory(c => new MultiClientTest(), _ => { })));
            using (var server = new Host(uri,container)) {
                for (int i = 0; i < threadCount; i++) {
                    var inner = i;
                    threads[i] = new Thread(() =>
                    {
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                        var client = new Client<ITest>(uri + "Test");
                        for (int j = 0; j < reps; j++) {
                            client.Proxy().VoidStringParam(inner.ToString()+ " rep: "+j.ToString());
                        }
                    });
                    threads[i].Start();
                }
                foreach (var thread in threads)
                    thread.Join();
            }
        
        }
    }

    public class MultiClientTest : ITest
    {
        public void VoidNoParam()
        {
            Thread.Sleep(20);
        }

        public void VoidStringParam(string str)
        {
            Thread.Sleep(50);
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                  
            Console.WriteLine(str);
         
        }

        public string StringNoParam()
        {
            return Guid.NewGuid().ToString();
        }


        public string Echo(string str)
        {
            return str;
        }
    }
}
