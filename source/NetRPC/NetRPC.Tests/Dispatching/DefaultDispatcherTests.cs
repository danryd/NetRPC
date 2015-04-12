namespace NetRPC.Tests.Dispatching
{
    using NetRPC.Dispatching;

    using Should;
    using System;
    public class DefaultDispatcherTests
    {
        private Dispatcher dispatcher = new DefaultDispatcher();

        public void DispatcherInvokesMethod()
        {
            var test = new Test();
            var result = dispatcher.Dispatch(typeof(ITest),"VoidNoParam", test, null);
            result.ShouldBeNull();
            test.Wascalled.ShouldBeTrue();
        }
        private class Test : ITest
        {
            private bool wascalled;

            public bool Wascalled
            {
                get { return wascalled; }
            }


            public void VoidNoParam()
            {
                wascalled = true;
            }

            public void VoidStringParam(string str)
            {
                throw new NotImplementedException();
            }

            public string StringNoParam()
            {
                throw new NotImplementedException();
            }
        }

    }



}