using Gloriosa.Core;
using Gloriosa.Exceptions;
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
                ClearOldCURVIEW();
            }
            viewType = type;
            CURVIEW = this;
            gOP = new GameObjectPool();
            lPOOL = new List<Resource>();
            Init();
        }

        private void ClearOldCURVIEW()
        {
            foreach (GameObject gm in CURVIEW.gOP.rPool)
                gm.Del();
            // Faire pareil pour tous les worlds.
            // Faire un switch pour Unload les resources selon leur type. (case resource is type) et supprimer l'élément dans la list.
            // CURVIEW.lPOOL.Clear();
            // Laisser le garbage collector s'occuper du reste.
        }

        public virtual void Init()
        {

        }

        public virtual void Frame()
        {
            if (viewType != ViewTypes.Menu)
            {
                foreach (World world in worlds)
                    world.objectPool.DoFrame();
            }
        }

        internal void Render()
        {
            if (viewType != ViewTypes.Menu)
            {
                foreach (World world in worlds)
                    world.objectPool.DoRender(RenderModes.World);
            }
            gOP.DoRender(RenderModes.UI);
        }

        /*public GameObject CreateGO(int worldID)
        {
            World? w = CURVIEW.worlds.Find(w => w.worldID == worldID);
            if (w != null)
            {
                GameObject gm = new GameObject();
                w.objectPool.NewGameObject(gm);
                gm.worldScope = true;
                return gm;
            }
            throw new UnknownWorldException("Cannot create a GameObject instance in an non-existing world.");
        }

        public GameObject CreateGO()
        {
            GameObject gm = new GameObject();
            gOP.NewGameObject(gm);
            return gm;
        }*/
    }
}
