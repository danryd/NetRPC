namespace NetRPC.Tests.Server
{
    using NetRPC.Server;
    using NetRPC.Server;
    using NetRPC.Serialization;
    using Should;
    public class PipelineTests
    {
        private PipeTest pipetest;
        private Pipeline pipeline;
        public PipelineTests()
        {
            pipetest = new PipeTest();
            pipeline = new Pipeline(typeof(IPipeTest),new JsonSerializer(), new DelegateServiceFactory(() => pipetest, _ => { }), new DefaultInvoker());

        }
        public void PipeCallsService()
        {
            var rq = MessageHelper.GenerateJsonRequest("Echo", new[] {"eko" } );

            pipeline.Handle(rq);
            pipetest.str.ShouldEqual("eko"); 
        }
        public void PipeCallReturnsResponse()
        {
            var rq = MessageHelper.GenerateJsonRequest("Echo", new[] { "eko" });
            var response = pipeline.Handle(rq);
            response.ShouldContain("eko");
        }
    }
    public interface IPipeTest
    {
        string Echo(string str);
    }
    public class PipeTest : IPipeTest
    {
        public string str;
        public string Echo(string str)
        {
            this.str = str;
            return str;
        }
    }
}
