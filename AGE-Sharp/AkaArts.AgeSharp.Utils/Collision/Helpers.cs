using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AkaArts.AgeSharp.Utils.Collision
{
    public static class Helpers
    {
        private const double Epsilon = 1e-10;

        public static bool IsZero(this double d)
        {
            return Math.Abs(d) < Epsilon;
        }

        public static bool IsZero(this float d)
        {
            return Math.Abs(d) < Epsilon;
        }

        public static float Cross(this Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        public static float Mult(this Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

    }

    public struct CollisionResult
    {
        public bool intersects;
        public Vector2 location;
    }

}
