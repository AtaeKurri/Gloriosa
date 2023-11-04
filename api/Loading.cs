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
        public static void LoadTexture(string filePath, string resourceName, bool shouldResourceBeLocal=false)
        {
            Texture tex = Raylib.LoadTexture(filePath);
            _ = new Resource(resourceName, tex, shouldResourceBeLocal);
        }

        public static void LoadShader(string filePath, string shaderName, bool shouldResourceBeLocal = false)
        {
            Shader sha = Raylib.LoadShader(filePath, shaderName);
            _ = new Resource(shaderName, sha, shouldResourceBeLocal);
        }

        public static void LoadFont(string filePath, string fontName, bool shouldResourceBeLocal = false)
        {
            Font fnt = Raylib.LoadFont(filePath);
            Raylib.SetTextureFilter(fnt.texture, TextureFilter.TEXTURE_FILTER_BILINEAR);
            _ = new Resource(fontName, fnt, shouldResourceBeLocal);
        }

        public static void LoadModel(string filePath, string modelName, bool shouldResourceBeLocal = false)
        {
            Model mod = Raylib.LoadModel(filePath);
            _ = new Resource(modelName, mod, shouldResourceBeLocal);
        }
    }
}
