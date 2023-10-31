using Gloriosa.Core;
using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Menus
{
    public abstract class Menu : GameObject
    {
        public Menu()
        {
            TPOOL.NewGameObject(this);
        }

        public void OnInput(KeyboardKey pressedKey)
        {

        }
    }
}
