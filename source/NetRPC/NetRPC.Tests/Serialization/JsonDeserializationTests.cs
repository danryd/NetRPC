

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
            var request = serializer.DeserializeRequest(payload);
            request.Method.ShouldEqual("Echo");
            request.CallId.ShouldEqual(callId);
            request.Parameters.Length.ShouldEqual(0);

        }
        public void CanDeserializeRequestParameterRequestTest()
        {
            var callId = Guid.NewGuid();
            string payload = "{\"Version\": \"0.5\", \"Method\": \"Echo\", \"Parameters\": [{\"Type\":\"string\",\"Value\":\"hej\"}], \"CallId\":\"" + callId.ToString() + "\" }";
            var request = serializer.DeserializeRequest(payload);
            request.Parameters.Length.ShouldEqual(1);;
            request.Parameters[0].Type.ShouldEqual("string");
            request.Parameters[0].Value.ShouldEqual("hej");

        }
        //public void CanDeserializeStringParamterTest() {
        //    var parameter = "{\"Type\":\"string\",\"Value\":\"hej\"}";
        //    var obj = serializer.Parameter(parameter);
        //    obj.ShouldBeType<string>();
        //    obj.ShouldEqual("hej");
        //}
    }
}
