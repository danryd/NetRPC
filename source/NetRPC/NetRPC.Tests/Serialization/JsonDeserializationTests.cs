

namespace NetRPC.Tests.Serialization
{
    using NetRPC.Serialization;
    using Should;
    using System;
    using System.Text;
    class JsonDeserializationTests
    {
        private Serializer serializer = new JsonSerializer();

        public void CanDeserializeRequestTest() {
            var callId =  Guid.NewGuid();
            string payload = "{\"Version\": \"0.5\", \"Method\": \"Echo\", \"Parameters\": [], \"CallId\":\"" +callId.ToString() + "\" }";
            var request = serializer.DeserializeRequest(Encoding.UTF8.GetBytes(payload));
            request.Method.ShouldEqual("Echo");
            request.CallId.ShouldEqual(callId);
            request.Parameters.Length.ShouldEqual(0);

        }
    }
}
