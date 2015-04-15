namespace NetRPC.Tests
{
    using NetRPC.Hosting;
using NetRPC.Invocation;
using NetRPC.Serialization;
using Should;
    public class PipelineTests
    {
        private Pipeline pipeline = new Pipeline(new JsonSerializer(), new DelegateServiceFactory(() => null, _ => { }), new DefaultInvoker());
    }
}
