using Gloriosa.Core;
using Raylib_CsLo.InternalHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Views
{
    public enum ViewTypes
    {
        Menu,
        Stage
    }

    public class View
    {
        public readonly ViewTypes viewType;

        public List<World> worlds;
        public GameObjectPool gOP;
        public List<Resource> lPOOL;

        public View(ViewTypes type)
        {
            if (CURVIEW != null)
            {
                // Clear l'ancienne view, libérer les resources locales, del les GameObject, et juste libérer l'intégralité de la View précédente quoi.
            }
            viewType = type;
            CURVIEW = this;
        }

        public virtual void Init()
        {

        }

        public virtual void Frame()
        {
            foreach (World world in worlds)
                world.objectPool.DoFrame();
        }

        internal void Render()
        {
            foreach (World world in worlds)
            {
                world.objectPool.DoRender(RenderModes.World);
            }
            gOP.DoRender(RenderModes.UI);
        }
    }
}
