using Raylib_CsLo;
using Raylib_CsLo.InternalHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Core
{
    public enum GameObjectStatus
    {
        Free,
        Active,
        Dead,
        Killed
    }

    public enum GameObjectGroup
    {
        GHOST,
        ENEMY_BULLET,
        ENEMY,
        PLAYER_BULLET,
        PLAYER,
        INDES,
        ITEM
    }

    public enum GameObjectLayer
    {
        BACKGROUND = 0,
        ENEMIES = 100,
        PLAYER_BULLETS = 200,
        PLAYER = 300,
        ITEMS = 400,
        ENEMY_BULLETS = 500,
        TOPMOST = 600,
    }

    public enum RenderModes
    {
        Background,
        World,
        UI
    }

    public class GameObject
    {
        public GameObjectStatus status = GameObjectStatus.Active;
        public uint uid;

        public int world;

        public Vector2 lastXY = Vector2.Zero;
        public Vector2 XY = Vector2.Zero;

        public GameObjectGroup group = GameObjectGroup.GHOST;
        public bool bound = true;
        public bool colli = true;
        public bool colliRect = false;
        public Vector2 colliAB = Vector2.Zero;

        public GameObjectLayer baseLayer = GameObjectLayer.ENEMIES;
        public float layer = 0.0f;
        public Vector2 scale = Vector2.One;
        public float rot = 0f;
        public float omega = 0f;
        public BlendMode blendMode = BlendMode.BLEND_ALPHA;

        public ulong timer = 0;
        public bool hide = false;
        public bool navi = false;
        public bool ignorePause = false;
        public bool paused = false;

        public bool onTop = false;
        public Texture? img = null;

        public bool worldScope = true;
        public RenderModes renderMode;
        public GameObjectPool pool;

        /// <summary>
        /// Creates a new GameObject instance living outside of a world.
        /// Will render on top of the worlds.
        /// </summary>
        /// <param name="onTop">If true, will always render on top of the worlds. If false, before the worlds.
        /// This can be changed at runtime with the <see cref="onTop"/> parameter.</param>
        public GameObject(bool onTop)
        {
            // Create the GameObject outside of a world scope, will always be rendered on top of the worlds.
            pool = TPOOL;
            pool.NewGameObject(this);
            worldScope = false;
        }

        /// <summary>
        /// Creates a new GameObject instance inside a world.
        /// </summary>
        /// <param name="worldID">The worldID in which this object will live. If not specified, will be 0.</param>
        public GameObject(int worldID=0)
        {
            World? w = WORLDS.Find(w => w.worldID == worldID);
            if (w != null)
            {
                pool = w.objectPool;
                pool.NewGameObject(this);
            }
            worldScope = true;
        }

        public virtual void Init()
        {

        }

        /// <summary>
        /// This method is called each frame.
        /// </summary>
        /// <param name="deltaTime">Time (in seconds) since the last frame.</param>
        public virtual void Frame(float deltaTime)
        {
            if (paused || !GameObjectPool.IsValid(this))
                return;

            rot += omega;
            if (!pool.BoundCheck(this))
            {
                Del();
                return;
            }
            UpdateTimer();
        }

        public virtual void Del()
        {
            if (status != GameObjectStatus.Active)
                return;
            status = GameObjectStatus.Dead;
            pool.RemoveObject(this);
        }

        public virtual void Kill()
        {
            if (status != GameObjectStatus.Active)
                return;
            status = GameObjectStatus.Killed;
            pool.RemoveObject(this);
        }

        /// <summary>
        /// This method is called when another object collides with this one.
        /// </summary>
        /// <param name="other">The GameObject which collided with this one.</param>
        public virtual void Colli(GameObject other)
        {

        }

        /// <summary>
        /// This method is called each frame inside the render scope.
        /// Will not run if the object is hidden.
        /// </summary>
        public virtual void Render()
        {

        }

        private void UpdateTimer()
        {
            timer++;
        }

        public bool IsInRect(int l, int r, int b, int t)
        {
            Debug.Assert(r >= l && t >= b);
            return (XY.X >= l && XY.X <= r && XY.Y >= b && XY.Y <= t);
        }

        /// <summary>
        /// Change the Render Mode of the object.
        /// </summary>
        /// <param name="rm"><see cref="RenderModes"/></param>
        public void SetRenderMode(RenderModes rm)
        {
            renderMode = rm;
        }
    }
}
