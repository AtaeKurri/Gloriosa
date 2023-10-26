using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Gloriosa.Core;
using Raylib_CsLo;

namespace Gloriosa.api
{
    public static partial class api
    {
        public static void Render(string resourceName, Vector2 position, float rot, float scale, Color color)
        {
            APP.CheckRenderScope();
            Texture? tex = Resource.FindResource<Texture?>(resourceName);
            if (tex == null)
                return;
            Raylib.DrawTextureEx((Texture)tex, position, rot, scale, color);
        }

        public static void RenderRect(string resourceName, Vector2 position, Vector2 size, Color color)
        {
            APP.CheckRenderScope();
            Texture? tex = Resource.FindResource<Texture?>(resourceName);
            if (tex == null)
                return;
            Raylib.DrawTexturePro((Texture)tex,
                new Rectangle(0, 0, ((Texture)tex).width, ((Texture)tex).height),
                new Rectangle(position.X, position.Y, ((Texture)tex).width, ((Texture)tex).height),
                new Vector2(0, 0),
                0,
                color);
        }

        public static void RenderText(string fontName, string text, Vector2 position, float fontSize, float spacing, Color color)
        {
            APP.CheckRenderScope();
            Font? fnt = Resource.FindResource<Font?>(fontName);
            if (fnt == null)
                return;
            Raylib.DrawTextEx((Font)fnt, text, position, fontSize, spacing, color);
        }

        public static void RenderModel(string resourceName, Vector3 position, float scale, Color color)
        {
            APP.CheckRenderScope();
            Model? mod = Resource.FindResource<Model?>(resourceName);
            if (mod == null)
                return;
            Raylib.DrawModel((Model)mod, position, scale, color);
        }
    }
}
