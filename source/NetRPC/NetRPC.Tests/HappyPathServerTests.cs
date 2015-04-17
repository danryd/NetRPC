

namespace NetRPC.Tests
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Text;
    using NetRPC.Server;
    using NetRPC.Serialization;
    using Should;
    using NetRPC.Client;
    public class HappyPathServerTests
    {
        private string happyPathURI = "http://localhost:{0}/";
        private ISerializer serializer = new JsonSerializer();
        private static int port = 9000;
        public HappyPathServerTests()
        {
            port++;
        }
        public void CanCallVoidMethod()
        {
            var payload = GetCall("Call");
            var result = TestMethod(payload);
            result.ShouldContain("\"Result\":null");
        }
        public void CanCallComplex()
        {
            var payload = GetCall("Complex", new object[] { new Complex { Data = "hey", Value = 4 } });
            var result = TestMethod(payload);
            result.ShouldContain("hey4");

        }
        public void CanCallWithProxy()
        {

            var container = new ServiceContainer();
            var factory = new DelegateServiceFactory(() => { return new HappyPathService(); }, _ => { return; });
            container.AddEndpoint(new Endpoint("Happy", typeof(IHappyPath), factory));

            using (var host = new HttpListenerHost(string.Format(happyPathURI, port), container))
            {
                var client = new Client<IHappyPath>(string.Format(happyPathURI, port) + "Happy");
                var result = client.Proxy().Complex(new Complex { Data = "hej", Value = 6004 });
                result.Value.ShouldEqual(12008);
                result.Data.ShouldEqual("hej6004");
            }

        }
        private string TestMethod(string payload)
        {
            var container = new ServiceContainer();
            var factory = new DelegateServiceFactory(() => { return new HappyPathService(); }, _ => { return; });
            container.AddEndpoint(new Endpoint("Happy", typeof(IHappyPath), factory));

            using (var host = new HttpListenerHost(string.Format(happyPathURI,port), container))
            {
                return Execute(payload);
            }
        }

        private string Execute(string payload)
        {
            var client = WebRequest.Create(string.Format(happyPathURI,port) + "Happy");
            client.Method = "POST";
            client.ContentType = Constants.ContentType;
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
                Version = Constants.Version,
                Parameters = parameters == null ? null : parameters.Select(p => serializer.SerializeToParameter(p)).ToArray()
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
