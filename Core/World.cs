using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Core
{
    public sealed class World
    {
        internal GameObjectPool objectPool;
        public int worldID = 0;
        // 0, 0 is at the top-left corner of the window.
        public Vector2 positionOnScreen = new Vector2(6, 16);
        public bool paused = false;
        public Vector4 worldBounds;
        public Vector4 worldSize = new Vector4(-192, 192, -224, 224);

        /// <summary>
        /// Creates a new world inside a view.
        /// </summary>
        /// <param name="_worldSize">Vector4 : left, right, bottom, top</param>
        /// <param name="_worldID">Unique ID for this world.</param>
        public World(Vector4 _worldSize, int _worldID = 0) : this(_worldID)
        {
            worldSize = _worldSize;
            // Génère un world avec un worldID et les paramètres de taille. Prend les param de taille par défaut si pas spécifiés.
        }

        /// <summary>
        /// Creates a new world inside a view. Takes default size parameters.
        /// </summary>
        /// <param name="_worldID">Unique ID for this world.</param>
        public World(int _worldID=0)
        {
            if (CURVIEW.worlds.Find(w => w.worldID == _worldID) != null)
                return;
            worldID = _worldID;
            objectPool = new GameObjectPool(this);
            CURVIEW.worlds.Add(this);
        }

        public Vector2 WorldToScreen()
        {
            return new Vector2(0, 0);
        }

        public Vector2 ScreenToWorld()
        {
            return new Vector2(0, 0);
        }

        /// <summary>
        /// Set the boundary for game objects.
        /// </summary>
        /// <param name="l">Left position (local)</param>
        /// <param name="r">Right position (local)</param>
        /// <param name="b">Bottom position (local)</param>
        /// <param name="t">Top position (local)</param>
        public void SetBounds(int l, int r, int b, int t)
        {
            worldBounds = new Vector4(l, r, b, t);
        }

        /// <summary>
        /// Pause the whole world. Will freeze everything inside it (unless a gameobject is ignoring pause.)
        /// </summary>
        /// <param name="pausing">Boolean for pausing or unpausing the game.</param>
        public void PauseWorld(bool pausing)
        {
            paused = pausing;
            foreach (GameObject go in objectPool.m_Pool)
                go.worldPaused = paused;
        }

        /// <summary>
        /// Returns a world instance given a world ID.
        /// </summary>
        /// <param name="worldID">The ID of the world you want.</param>
        /// <returns>A world instance. null if a world with this ID doesn't exist.</returns>
        public static World? GetWorld(int worldID)
        {
            return CURVIEW.worlds.Find(w => w.worldID == worldID);
        }

        public void DoCollisionChecks()
        {
            CollisionChecks(GameObjectGroup.PLAYER, GameObjectGroup.ENEMY_BULLET);
            CollisionChecks(GameObjectGroup.PLAYER, GameObjectGroup.INDES);
            CollisionChecks(GameObjectGroup.PLAYER, GameObjectGroup.ENEMY);
            CollisionChecks(GameObjectGroup.ENEMY, GameObjectGroup.PLAYER_BULLET);
            CollisionChecks(GameObjectGroup.ITEM, GameObjectGroup.PLAYER);
        }

        public void CollisionChecks(GameObjectGroup groupA, GameObjectGroup groupB)
        {
            foreach (GameObject goA in objectPool.m_Pool.FindAll(x => x.group == groupA))
            {
                if (!goA.checkColli)
                    return;

                foreach (GameObject goB in objectPool.m_Pool.FindAll(y => y.group == groupB))
                {
                    if (!goB.checkColli)
                        continue;
                    goA.CollisionCheck(goB);
                }
            }
        }
    }
}
