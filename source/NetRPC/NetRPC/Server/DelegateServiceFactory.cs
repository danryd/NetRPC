namespace NetRPC.Server
{
    using System;
    public class DelegateServiceFactory : IServiceFactory
    {
        Func<OperationContext,object> factory;
        Action<object> onRelease;
        public DelegateServiceFactory(Func<OperationContext,object> factory, Action<object> onRelease)
        {
            this.factory = factory;
            this.onRelease = onRelease;
        }
        public object Create(OperationContext operationContext)
        {
            return factory(operationContext);
        }

        public void Release(object o)
        {
            onRelease(o);
        }
    }
}
