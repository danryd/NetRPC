using System.Text;

namespace System
{
    public static class Extensions
    {
        public static byte[] ToByteArray(this string target){
            return Encoding.UTF8.GetBytes(target);
        }
    }
}
