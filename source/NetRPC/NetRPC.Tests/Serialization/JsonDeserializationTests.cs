

namespace NetRPC.Tests.Serialization
{
    using NetRPC.Serialization;
    using Should;
    using System;
    using System.Text;
    class JsonDeserializationTests
    {
        private ISerializer serializer = new JsonSerializer();

        public void CanDeserializeRequestTest() {
            var callId =  Guid.NewGuid();
            string payload = "{\"Version\": \"0.5\", \"Method\": \"Echo\", \"Parameters\": [], \"CallId\":\"" +callId.ToString() + "\" }";
            var request = serializer.Deserialize(payload);
            request.Method.ShouldEqual("Echo");
            request.CallId.ShouldEqual(callId);
            request.Parameters.Length.ShouldEqual(0);

        }
        public void CanDeserializeRequestParameterRequestTest()
        {
            var callId = Guid.NewGuid();
            string payload = "{\"Version\": \"0.5\", \"Method\": \"Echo\", \"Parameters\": [{\"Type\":\"string\",\"Value\":\"hej\"}], \"CallId\":\"" + callId.ToString() + "\" }";
            var request = serializer.Deserialize(payload);
            request.Parameters.Length.ShouldEqual(1);;
            request.Parameters[0].Type.ShouldEqual("string");
            request.Parameters[0].Value.ShouldEqual("hej");

        }
        public void CanDeserializeComplexRequestParameterRequestTest()
        {
            var callId = Guid.NewGuid();
            string payload = "{\"Version\": \""+Constants.Version+"\", \"Method\": \"Echo\", \"Parameters\": [{\"Type\":\"NetRPC.Tests.Complex\",\"Value\":\"{\\\"Data\\\":4}\"}], \"CallId\":\"" + callId.ToString() + "\" }";
            var request = serializer.Deserialize(payload);
            request.Parameters.Length.ShouldEqual(1); ;
            request.Parameters[0].Type.ShouldEqual("NetRPC.Tests.Complex");
            request.Parameters[0].Value.ShouldEqual("{\"Data\":4}");
            

        }
        public void CanSerializeToSimpleJsonTest()
        {
            var callId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            string expected = "{\"Version\":\""+Constants.Version+"\",\"Method\":\"Echo\",\"CallId\":\"" + callId.ToString() + "\",\"SessionId\":\"" + sessionId.ToString() + "\",\"Headers\":null,\"Parameters\":[{\"Type\":\"string\",\"Value\":\"hej\"}],\"Result\":null,\"Error\":null}";
            var request = new Message
                {
                    Version = Constants.Version,
                    CallId = callId,
                    SessionId = sessionId,
                    Method = "Echo",
                    Parameters = new Parameter[] {new Parameter {Type = "string", Value = "hej"}},

                };
            var json = serializer.Serialize(request);
            json.ShouldEqual(expected);
        }

        public void CanSerializeAndDeserializeComplexType() {
            var complex = new Complex { Data = 42 };
            var parameter = serializer.SerializeToParameter(complex);
            var actual = (Complex)serializer.DeserializeParameter(parameter);
            actual.Data.ShouldEqual(42);
        }
        public void CanSerializeComplexJsonTest()
        {
            var callId = Guid.NewGuid();
            var sessionId = Guid.NewGuid();
            string expected = "{\"Version\":\"0.6\",\"Method\":\"Echo\",\"CallId\":\"" + callId.ToString() + "\",\"SessionId\":\"" + sessionId.ToString() + "\",\"Headers\":null,\"Parameters\":[{\"Type\":\"NetRPC.Tests.Serialization.Complex, NetRPC.Tests\",\"Value\":\"{\\\"Data\\\":4}\"}],\"Result\":null,\"Error\":null}";
            var request = new Message
            {
                Version = Constants.Version,
                CallId = callId,
                SessionId = sessionId,
                Method = "Echo",
                Parameters = new Parameter[] {serializer.SerializeToParameter(new Complex{Data=4} )},

            };
            var json = serializer.Serialize(request);
            json.ShouldEqual(expected);
        }
    }

    public class Complex
    {
        public int Data { get; set; }
    }
}
