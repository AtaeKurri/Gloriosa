using Gloriosa.Exceptions;
using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Core
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
                bool tmp = ClearOldCURVIEW();
            }
            viewType = type;
            CURVIEW = this;
            gOP = new GameObjectPool();
            lPOOL = new List<Resource>();
            Init();
        }

        private bool ClearOldCURVIEW()
        {
            CURVIEW = null;
            foreach (GameObject gm in CURVIEW.gOP.rPool)
                gm.Del();
            foreach (World w in CURVIEW.worlds)
                foreach (GameObject go in w.objectPool.rPool)
                    go.Del();
            foreach (Resource r in CURVIEW.lPOOL)
            {
                if (r.resource is Texture)
                    Raylib.UnloadTexture((Texture)r.resource);
                else if (r.resource is Shader)
                    Raylib.UnloadShader((Shader)r.resource);
                else if (r.resource is Font)
                    Raylib.UnloadFont((Font)r.resource);
                else if (r.resource is Model)
                    Raylib.UnloadModel((Model)r.resource);
            }
            CURVIEW.lPOOL.Clear();
            // Laisser le garbage collector s'occuper du reste.
            return true;
        }

        /// <summary>
        /// Creates a new world instance and adds it to the world pool.<br></br>
        /// Will fail if the View's Type is not a Stage or if a world with the same ID already exists.
        /// </summary>
        /// <param name="worldID">World unique id.</param>
        /// <returns>The world Instance. Null if failing.</returns>
        public World CreateWorld(int worldID)
        {
            if (viewType != ViewTypes.Stage)
                return null;
            else if (worlds.Find(w => w.worldID == worldID) != null)
                return null;
            World w = new World(worldID);
            worlds.Add(w);
            return w;
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
