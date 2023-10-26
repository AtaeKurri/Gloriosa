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
        public static bool IsKeyPressed(KeyboardKey key)
        {
            return Raylib.IsKeyPressed(key);
        }

        public static bool IsKeyDown(KeyboardKey key)
        {
            return Raylib.IsKeyDown(key);
        }

        public static bool IsKeyReleased(KeyboardKey key)
        {
            return Raylib.IsKeyReleased(key);
        }

        public static bool IsKeyUp(KeyboardKey key)
        {
            return Raylib.IsKeyUp(key);
        }

        public static int IsKeyDown()
        {
            return Raylib.GetKeyPressed();
        }
    }
}
