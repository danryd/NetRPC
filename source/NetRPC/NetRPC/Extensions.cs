using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class Extensions
    {
        public static byte[] ToByteArray(this string target){
            return Encoding.UTF8.GetBytes(target);
        }
    }
}
