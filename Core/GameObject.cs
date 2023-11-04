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

    public enum CollisionShape
    {
        Rectangle,
        Circle,
        Polygon /// WARNING: NOT SUPPORTED
    }

    public class GameObject
    {
        public GameObjectStatus status = GameObjectStatus.Active;
        public uint uid;

        public int world;

        public Vector2 lastPosition = Vector2.Zero;
        public Vector2 position = Vector2.Zero;

        public GameObjectGroup group = GameObjectGroup.GHOST;
        public bool bound = true;
        public bool checkColli = true;
        public bool colliIsRect = false;
        public Vector2 colliSize = Vector2.Zero;

        public GameObjectLayer baseLayer = GameObjectLayer.ENEMIES;
        public float layer = 0.0f;
        public Vector2 scale = Vector2.One;
        public float rot = 0f;
        public float omega = 0f;
        public BlendMode blendMode = BlendMode.BLEND_ALPHA;

        public ulong timer = 0;
        public bool hidden = false;
        public bool autoRot = false;
        public bool ignorePause = false;
        public bool paused = false;
        public bool worldPaused = false;

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

        /// <summary>
        /// Should pause it returning true.
        /// </summary>
        /// <returns></returns>
        private bool ShouldPause()
        {
            if (ignorePause && !GameObjectPool.IsValid(this))
                return false;
            if (worldPaused || paused)
                return true;
            return false;
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
            if (ShouldPause())
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
        /// This method is called when another object collides with this one.<br></br>
        /// Won't detect collision if the object and/or the world is paused.
        /// </summary>
        /// <param name="other">The GameObject which collided with this one.</param>
        public virtual void Colli(GameObject other)
        {
            if (ShouldPause())
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
            RenderCollisionShape(); // A retirer dans la version finale, c'est pour debuguer.
        }

        /// <summary>
        /// Render the collision shape of the object, intended for debugging purposes.
        /// </summary>
        public void RenderCollisionShape()
        {

        }

        private void UpdateTimer()
        {
            timer++;
        }

        public bool IsInRect(int l, int r, int b, int t)
        {
            Debug.Assert(r >= l && t >= b);
            return (position.X >= l && position.X <= r && position.Y >= b && position.Y <= t);
        }

        /// <summary>
        /// Change the Render Mode of the object.
        /// </summary>
        /// <param name="rm"><see cref="RenderModes"/></param>
        public void SetRenderMode(RenderModes rm)
        {
            renderMode = rm;
        }

        /// <summary>
        /// Sets the collision shape for the object. 
        /// </summary>
        /// <param name="shape">The shape of this object's collider</param>
        /// <param name="args">Arguments for the collision shape.</param>
        public void SetCollisionShape(CollisionShape shape, params object[] args)
        {

        }

        public void CollisionCheck(GameObject other)
        {
            if (colliIsRect && other.colliIsRect)
            {
                if ((position.X < (other.position.X + other.colliSize.X) && (position.X + colliSize.X) > other.position.X) &&
                    (position.Y < (other.position.Y + other.colliSize.Y) && (position.Y + colliSize.Y) > other.position.Y))
                {
                    Colli(other);
                    return;
                }
            }
            else if (colliIsRect && !other.colliIsRect)
            {
                // Faire le check si l'un est rectangle est l'autre ovale.
            }
            else
            {
                // Faire le check si les deux sont ovales.
            }
        }
    }
}
