using Gloriosa.Exceptions;
using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
                bool tmp = ClearOldCURVIEW();
            }
            viewType = type;
            CURVIEW = this;
            worlds = new List<World>();
            gOP = new GameObjectPool();
            lPOOL = new List<Resource>();
            Init();
        }

        private bool ClearOldCURVIEW()
        {
            foreach (GameObject gm in CURVIEW.gOP.m_Pool)
                gm.status = GameObjectStatus.Free;
            foreach (World w in CURVIEW.worlds)
                foreach (GameObject go in w.objectPool.m_Pool)
                    go.status = GameObjectStatus.Free;
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
            CURVIEW = null;
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

        public static T CreateView<T>(params object[] args)
        {
            return (T)Activator.CreateInstance(typeof(T), args);
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

        /// <summary>
        /// Set the boundary for game objects in all existing worlds.
        /// </summary>
        /// <param name="l">Left position (local)</param>
        /// <param name="r">Right position (local)</param>
        /// <param name="b">Bottom position (local)</param>
        /// <param name="t">Top position (local)</param>
        public void SetBoundsAll(int l, int r, int b, int t)
        {
            foreach (World world in worlds)
                world.worldBounds = new Vector4(l, r, b, t);
        }
    }
}
