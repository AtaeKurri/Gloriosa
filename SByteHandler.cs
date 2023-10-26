using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa
{
    unsafe public static class SByteHandler
    {
        public static sbyte* ToSByte(string str)
        {
            byte[] bytes = new byte[Encoding.Default.GetByteCount(str) + 1];
            Encoding.Default.GetBytes(str, 0, str.Length, bytes, 0);

            fixed (byte* b = bytes)
            {
                sbyte* b2 = (sbyte*)b;
                return b2;
            }
        }

        public static string FromSByte(sbyte* str)
        {
            return new string(str);
        }
    }
}
