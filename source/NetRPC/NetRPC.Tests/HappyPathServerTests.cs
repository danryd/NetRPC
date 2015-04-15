

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
        public void CanCallVoidMethod()
        {
            var payload = GetCall("Call");
            var result = TestMethod(payload);
            result.ShouldContain("Void");
        }
        public void CanCallComplex()
        {
            var payload = GetCall("Complex", new object[] { new Complex { Data = "hey", Value = 4 } });
            var result = TestMethod(payload);
            result.ShouldContain("hey4");

        }
        private string TestMethod(string payload)
        {
            var container = new ServiceContainer();
            var factory = new DelegateServiceFactory(() => { return new HappyPathService(); }, _ => { return; });
            container.AddEndpoint(new Endpoint("Happy", typeof(IHappyPath), factory));

            using (var host = new HttpListenerHost(happyPathURI, container))
            {
                return Execute(payload);
            }
        }

        private string Execute(string payload)
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
            var respText = new StreamHandler().ReadToString(responseStream);
            return respText;
        }

        protected string GetCall(string method, object[] parameters = null)
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
            return;
        }


        public Complex Complex(Complex complex)
        {
            return new Complex { Data = complex.Data + complex.Value.ToString(), Value = complex.Value * 2 };
        }
    }

    public interface IHappyPath
    {
        void Call();

        Complex Complex(Complex complex);
    }
    public class Complex
    {
        public string Data { get; set; }
        public int Value { get; set; }
    }

}
