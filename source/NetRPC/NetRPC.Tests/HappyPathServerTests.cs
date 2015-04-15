

namespace NetRPC.Tests
{
    using System;
    using System.Net;
    using System.Text;
    using NetRPC.Hosting;
    using NetRPC.Serialization;
    using Should;
    public class HappyPathServerTests
    {
        private string happyPathURI = "http://localhost:9999/";
        private ISerializer serializer = new JsonSerializer();
        protected void CanCallVoidMethod()
        {
             var container = new ServiceContainer();
             var factory = new DelegateServiceFactory(() => { return new HappyPathService(); }, _ => { return; });
             container.AddEndpoint(new Endpoint("Happy", factory));
          
            using (var host = new HttpListenerHost(happyPathURI, container))
            {
                Execute(GetCall("Call"));
            }
        }

        private void Execute(string payload)
        {
            var client = WebRequest.Create(happyPathURI + "Happy");
            client.Method = "POST";
            client.ContentType = "rpc/json";
            var stream = client.GetRequestStream();
            stream.Write(payload.ToByteArray(), 0, payload.ToByteArray().Length);
            stream.Close();
            var resp = (HttpWebResponse)client.GetResponse();
            resp.StatusCode.ShouldEqual(HttpStatusCode.OK);
            var responseStream = resp.GetResponseStream();
            var respBytes = new byte[responseStream.Length];
            responseStream.Read(respBytes, 0, (int)responseStream.Length);
            Console.WriteLine(Encoding.UTF8.GetString(respBytes));
        }

        protected string GetCall(string method)
        {
            var request = new Request
            {
                CallId = Guid.NewGuid(),
                SessionId = Guid.NewGuid(),
                Method = method,
                Version = "0.5"
            };
            return serializer.SerializeRequest(request);
        }

    }
    public class HappyPathService : IHappyPath
    {

        public void Call()
        {
            throw new NotImplementedException();
        }
    }

    public interface IHappyPath
    {
        void Call();

    }
}
