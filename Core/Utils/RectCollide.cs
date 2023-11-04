using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Core.Utils
{
    internal class ColliRectVec2
    {
        public double x;
        public double y;

        internal ColliRectVec2(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        internal void Rotate(double theta)
        {
            x = x * Math.Cos(theta) - y * Math.Sin(theta);
            y = x * Math.Sin(theta) + y * Math.Cos(theta);
        }

        internal float GetMagnitude()
        {
            return MathF.Sqrt((float)(x * x + y * y));
        }

        internal ColliRectVec2 Add(double factor)
        {
            return new ColliRectVec2(this.x + factor, this.y + factor);
        }

        internal void Add(ColliRectVec2 factor)
        {

        }
    }

    internal class ColliRectLine
    {
        public ColliRectVec2 origin;
        public ColliRectVec2 direction;

        internal ColliRectLine(double _x, double _y, double dx, double dy)
        {
            origin = new ColliRectVec2(_x, _y);
            direction = new ColliRectVec2(dx, dy);
        }
    }

    internal class RectCollider
    {
        public Vector2 center = new Vector2(0, 0);
        public Vector2 size = new Vector2(20, 10);
        public double angle = 0.0f; // In deg.

        internal static ColliRectLine[] getAxis(RectCollider rect)
        {
            ColliRectVec2 OX = new ColliRectVec2(1, 0);
            ColliRectVec2 OY = new ColliRectVec2(0, 1);
            // Transform from deg to rad
            OX.Rotate(rect.angle * Math.PI / 180);
            OY.Rotate(rect.angle * Math.PI / 180);

            return new ColliRectLine[2] { 
                new ColliRectLine(rect.center.X, rect.center.Y, OX.x, OX.y),
                new ColliRectLine(rect.center.X, rect.center.Y, OY.x, OY.y)
            };
        }
    }
}
