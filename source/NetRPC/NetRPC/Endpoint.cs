using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetRPC
{
    public class Endpoint
    {
        private readonly Type contract;
        private readonly Func<object> factory;
        private readonly string relativeAddress;

        public Endpoint(Type contract, Func<object> factory, string relativeAddress)
        {
            this.contract = contract;
            this.factory = factory;
            this.relativeAddress = relativeAddress;
        }

        public object CreateInstance()
        {
            return factory();
        }

        internal void Handle(Stream inputStream, Stream outputStream)
        {
            throw new NotImplementedException();
        }
    }
}
