using Gloriosa.Core;
using Gloriosa.Exceptions;
using Gloriosa.IO;
using Raylib_CsLo;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa
{
    public struct GameSettings
    {
        public Vector2 canvasSize = new Vector2(1600, 900);
        public string windowTitle = "Gloriosa | v1.00";

        public GameSettings()
        {
        }
    }

    public delegate void EventHandler(AppFrame appf);

    public sealed class AppFrame
    {
        public bool isRunning = false;
        private bool isReady = false;
        public GameSettings m_Settings = new GameSettings();
        public RenderTexture target = new RenderTexture();
        private Camera2D screenCamera2D = new Camera2D();
        private Camera2D worldCamera2D = new Camera2D();
        private Camera3D BGCamera3D = new Camera3D();

        private float virtualRatio = 0.0f;
        private Rectangle sourceRect;
        private Rectangle destRect;

        public bool isInRenderScope = false;

        public event EventHandler GameInitialized;

        public AppFrame()
        {
            if (APP != null)
                throw new GameAlreadyRunningException("There is already a game instance running. Can't create new one.");
            isRunning = true;
            APP = this;
            GPOOL = new List<Resource>();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/engine.log", rollingInterval: RollingInterval.Day, buffered: true)
                .CreateLogger();
        }

        public AppFrame(string windowTitle) : this()
        {
            m_Settings.windowTitle = windowTitle;
        }

        public static List<World> getWorlds()
        {
            return CURVIEW.worlds;
        }

        public static Scoredata getScoredata()
        {
            return SCORE;
        }

        public void Init()
        {
            Raylib.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_VSYNC_HINT);
            Raylib.InitWindow((int)m_Settings.canvasSize.X, (int)m_Settings.canvasSize.Y, m_Settings.windowTitle);
            Raylib.SetTargetFPS(60);

            SetVirtualSize(new Vector2(853, 480));
            ResetCamera();

            isReady = true;
            Log.Information("Engine started.");
            GameInitialized?.Invoke(this);

            while (!GameShouldClose())
                Tick();

            Raylib.UnloadRenderTexture(target);
            Raylib.CloseWindow();
            return;
        }

        public bool GameShouldClose()
        {
            return Raylib.WindowShouldClose();
        }

        public bool IsGameReady()
        {
            return isReady;
        }

        public void SetVirtualSize(Vector2 virtualSize)
        {
            virtualRatio = (float)m_Settings.canvasSize.X / (float)virtualSize.X;
            target = Raylib.LoadRenderTexture((int)virtualSize.X, (int)virtualSize.Y);
            sourceRect = new Rectangle(0.0f, 0.0f, (float)target.texture.width, -(float)target.texture.height);
            destRect = new Rectangle(-virtualRatio, -virtualRatio, m_Settings.canvasSize.X + (virtualRatio * 2), m_Settings.canvasSize.Y + (virtualRatio * 2));
        }

        private void ResetCamera()
        {
            worldCamera2D.offset = new Vector2(0, 0);
            worldCamera2D.target = new Vector2(0, 0);
            worldCamera2D.rotation = 0.0f; // In degrees.
            worldCamera2D.zoom = 1.0f;

            screenCamera2D.offset = new Vector2(0, 0);
            screenCamera2D.target = new Vector2(0, 0);
            screenCamera2D.rotation = 0.0f; // In degrees.
            screenCamera2D.zoom = 1.0f;
        }

        public void Tick()
        {
            Update();

            Raylib.BeginTextureMode(target);
            Raylib.ClearBackground(Raylib.BLACK);
            isInRenderScope = true;
            Raylib.BeginMode3D(BGCamera3D);
            DrawBackground();
            Raylib.EndMode3D();
            Raylib.BeginMode2D(worldCamera2D);
            DrawView();
            Raylib.EndMode2D();
            DrawUI();
            isInRenderScope = false;
            Raylib.EndTextureMode();

            Raylib.BeginDrawing();
            Raylib.BeginMode2D(screenCamera2D);
            Raylib.DrawTexturePro(target.texture, sourceRect, destRect, new Vector2(0, 0), 0, Raylib.WHITE);
            Raylib.EndMode2D();
            Raylib.EndDrawing();
        }

        private void Update()
        {
            if (CURVIEW != null)
                CURVIEW.Frame();
        }

        private void DrawBackground()
        {
            //CURVIEW.RenderBackGround(RenderModes.Background);
            if (CURVIEW != null)
                CURVIEW.gOP.DoRender(RenderModes.Background);
        }

        private void DrawView()
        {
            if (CURVIEW != null)
                CURVIEW.Render();
        }

        private void DrawUI()
        {
            Raylib.DrawFPS(10, 10);
        }

        public void CheckRenderScope()
        {
            if (!isInRenderScope)
                throw new InvalidScopeException("This function cannot be used unless in Render Scope.");
        }
    }
}
