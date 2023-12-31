﻿using Raylib_CsLo;
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

        public List<GameObject> m_Pool;
        private uint nextUid = 0;
        private float nextLayer = 0.0f;

        public bool isProcessingFrame = false;
        public bool isRendering = false;

        public GameObjectPool()
        {
            m_Pool = new List<GameObject>();
        }

        public GameObjectPool(World world) : this()
        {
            parentWorld = world;
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

            if (parentWorld != null)
                parentWorld.DoCollisionChecks();
        }
        public void DoRender(RenderModes renderMode)
        {
            isRendering = true;
            foreach (GameObject gm in m_Pool.FindAll(x => x.renderMode == renderMode && !x.hidden))
                gm.Render();
            isRendering = false;
        }

        public void SortPool()
        {
            m_Pool.OrderBy(x => x.renderMode).ThenBy(x => x.baseLayer).ThenBy(x => x.layer);
        }

        public void NewGameObject(GameObject gm)
        {
            if (nextUid == objectLimit)
                return;
            gm.uid = nextUid;
            gm.layer = (int)gm.baseLayer + nextLayer;
            m_Pool.Add(gm);
            gm.Init();
            nextUid++;
            nextLayer += 0.001f;
        }

        public void RemoveObject(GameObject obj)
        {
            m_Pool.Remove(obj);
            obj.status = GameObjectStatus.Free;
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

            return (obj.position.X >= parentWorld.worldBounds.X && obj.position.X <= parentWorld.worldBounds.Y
                && obj.position.Y >= parentWorld.worldBounds.Z && obj.position.Y <= parentWorld.worldBounds.W);
        }

        public static bool IsValid(GameObject obj)
        {
            if (obj.status == GameObjectStatus.Active)
                return true;
            return false;
        }
    }
}
