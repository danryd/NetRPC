namespace NetRPC.Server
{
    using System;
    public class DelegateServiceFactory : IServiceFactory
    {
        Func<object> factory;
        Action<object> onRelease;
        public DelegateServiceFactory(Func<object> factory, Action<object> onRelease)
        {
            this.factory = factory;
            this.onRelease = onRelease;
        }
        public object Create()
        {
            return factory();
        }

        public void Release(object o)
        {
            onRelease(o);
        }
    }
}
