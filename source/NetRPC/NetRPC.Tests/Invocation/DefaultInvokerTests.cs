namespace NetRPC.Tests.Dispatching
{
    using NetRPC.Invocation;

    using Should;
    using System;
    public class DefaultDispatcherTests
    {
        private IServiceInvoker dispatcher = new DefaultInvoker();

        public void DispatcherInvokesVoidNoParam()
        {
            var test = new Test();
            var result = dispatcher.Dispatch(typeof(ITest),"VoidNoParam", test, null);
            result.ShouldBeNull();
            test.Wascalled.ShouldBeTrue();
        }
        public void DispatcherInvokesVoidStringParam()
        {
            var test = new Test();
            var result = dispatcher.Dispatch(typeof(ITest), "VoidStringParam", test,  new object[]{ "assert"});
            result.ShouldBeNull();
            test.VoidStringParamValue.ShouldEqual("assert");
        }
        public void DispatcherInvokesStringResult()
        {
            var test = new Test();
            var result = dispatcher.Dispatch(typeof(ITest), "StringNoParam", test, null);
            result.ShouldEqual("assert");
            
        }
        private class Test : ITest
        {
            private bool wascalled;

            public bool Wascalled
            {
                get { return wascalled; }
            }

            public string  VoidStringParamValue{ get; set; }
            public void VoidNoParam()
            {
                wascalled = true;
            }

            public void VoidStringParam(string str)
            {
                VoidStringParamValue = str;
            }

            public string StringNoParam()
            {
                return "assert";
            }
        }

    }



}