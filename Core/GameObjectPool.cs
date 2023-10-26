using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Core
{
    public sealed class GameObjectPool
    {
        const int objectLimit = 40_000;
        private World? parentWorld;

        private List<GameObject> m_Pool;
        private uint nextUid = 0;

        public bool isProcessingFrame = false;
        public bool isRendering = false;

        public GameObjectPool()
        {
            m_Pool = new List<GameObject>();
        }

        public GameObjectPool(World world)
        {
            parentWorld = world;
            m_Pool = new List<GameObject>();
        }

        public void ResetPool()
        {
            foreach (GameObject obj in m_Pool)
                obj.Del();
            m_Pool.Clear();
        }

        public void DoFrame()
        {
            float dt = Raylib.GetFrameTime();
            isProcessingFrame = true;
            foreach (GameObject gm in m_Pool)
            {
                gm.Frame(dt);
            }
            isProcessingFrame = false;
        }

        public void DoRender()
        {
            isRendering = true;
            // Assume the pool is already sorted by the override when using this function.
            // So, don't sort it.
            //m_Pool.Sort((obj1, obj2) => obj1.layer.CompareTo(obj2.layer));
            foreach (GameObject gm in m_Pool)
                gm.Render();
            isRendering = false;
        }

        public void DoRender(bool topRendering)
        {
            isRendering = true;
            if (!topRendering) // Assume the pool is already sorted by the first call of this function.
                m_Pool.Sort((obj1, obj2) => obj1.layer.CompareTo(obj2.layer));
            foreach (GameObject gm in m_Pool.FindAll(x => x.onTop == topRendering && !x.hide))
                gm.Render();
            isRendering = false;
        }

        public void NewGameObject(GameObject gm)
        {
            if (nextUid == objectLimit)
                return;
            gm.uid = nextUid;
            m_Pool.Add(gm);
            gm.Init();
            nextUid++;
        }

        /// <summary>
        /// Check the boundary for objects. Will take World.worldBounds into account instead of World.worldSize.
        /// </summary>
        /// <param name="obj">The game object to check.</param>
        /// <returns>True if inside the bounds of the world.</returns>
        public bool BoundCheck(GameObject obj)
        {
            if (parentWorld == null)
                return true;
            Debug.Assert(parentWorld.worldBounds.Y >= parentWorld.worldBounds.X && parentWorld.worldBounds.W >= parentWorld.worldBounds.Z);

            return (obj.XY.X >= parentWorld.worldBounds.X && obj.XY.X <= parentWorld.worldBounds.Y
                && obj.XY.Y >= parentWorld.worldBounds.Z && obj.XY.Y <= parentWorld.worldBounds.W);
        }

        public static bool IsValid(GameObject obj)
        {
            if (obj.status == GameObjectStatus.Active)
                return true;
            return false;
        }

        public void RemoveObject(GameObject obj)
        {
            m_Pool.Remove(obj);
            obj.status = GameObjectStatus.Free;
        }
    }
}
