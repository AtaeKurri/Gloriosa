using Gloriosa.Exceptions;
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
        Deleted,
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

        /// <summary>
        /// Creates a new GameObject instance inside a world.
        /// </summary>
        /// <param name="_renderMode">If <see cref="RenderModes.UI"/>, will ignore <paramref name="worldID"/>.</param>
        /// <param name="worldID">The worldID in which this object will live. If not specified, will be 0.</param>
        public GameObject(RenderModes _renderMode, int worldID=0)
        {
            renderMode = _renderMode;
            if (renderMode == RenderModes.UI)
            {
                CURVIEW.gOP.NewGameObject(this);
                worldScope = false;
            }
            else
            {
                World? w = CURVIEW.worlds.Find(w => w.worldID == worldID);
                if (w == null)
                    throw new UnknownWorldException("Cannot create a GameObject instance in a non-existing world.");
                w.objectPool.NewGameObject(this);
                worldScope = true;
            }
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
            if (worldScope)
            {
                if (!CURVIEW.worlds[world].objectPool.BoundCheck(this))
                {
                    Del();
                    return;
                }
            }
            UpdateTimer();
        }

        public virtual void Del()
        {
            if (DoDelKill(GameObjectStatus.Deleted))
                return;
        }

        public virtual void Kill()
        {
            if (DoDelKill(GameObjectStatus.Killed))
                return;
        }

        private bool DoDelKill(GameObjectStatus stat)
        {
            if (status != GameObjectStatus.Active)
                return false;
            status = stat;
            if (renderMode != RenderModes.UI)
                CURVIEW.worlds[world].objectPool.RemoveObject(this);
            else
                CURVIEW.gOP.RemoveObject(this);
            return true;
        }

        /// <summary>
        /// This method is called when another object collides with this one.
        /// </summary>
        /// <param name="other">The GameObject which collided with this one.</param>
        public virtual void Colli(GameObject other)
        {
            if (!GameObjectPool.IsValid(this))
                return;
        }

        /// <summary>
        /// This method is called each frame inside the render scope.
        /// Will not run if the object is hidden.
        /// </summary>
        public virtual void Render()
        {
            if (!GameObjectPool.IsValid(this))
                return;
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
