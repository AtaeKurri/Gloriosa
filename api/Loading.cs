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
        public static void LoadTexture(string filePath, string resourceName)
        {
            Texture tex = Raylib.LoadTexture(filePath);
            _ = new Resource(resourceName, tex);
        }

        public static void LoadShader(string filePath, string shaderName)
        {
            Shader sha = Raylib.LoadShader(filePath, shaderName);
            _ = new Resource(shaderName, sha);
        }

        public static void LoadFont(string filePath, string fontName)
        {
            Font fnt = Raylib.LoadFont(filePath);
            _ = new Resource(fontName, fnt);
        }

        public static void LoadModel(string filePath, string modelName)
        {
            Model mod = Raylib.LoadModel(filePath);
            _ = new Resource(modelName, mod);
        }
    }
}
