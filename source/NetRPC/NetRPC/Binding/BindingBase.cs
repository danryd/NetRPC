namespace NetRPC.Binding
{
    using NetRPC.Invocation;
    using NetRPC.Serialization;
    public abstract class BindingBase
    {
        public BindingBase(ISerializer serializer, IServiceInvoker dispatcher)
        {

        }
    }
}
