using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.api
{
    public static partial class api
    {
        public static Color GColor(int r, int g, int b, int a=255)
        {
            return new Color(r, g, b, a);
        }
    }
}
