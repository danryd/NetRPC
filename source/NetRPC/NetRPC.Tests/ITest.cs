using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.Tests
{
    interface ITest
    {
        void VoidNoParam();
        void VoidStringParam(string str);
        string StringNoParam();
    }


}
