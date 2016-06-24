using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AkaArts.AgeSharp.Utils.Collision
{
    public static class Helpers
    {
        private const double EpsilonD = 1e-10d;
        private const float EpsilonF = 1e-5f;

        public static bool IsZero(this double d)
        {
            return Math.Abs(d) < EpsilonD;
        }

        public static bool IsZero(this float d)
        {
            return Math.Abs(d) < EpsilonF;
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

    public class CollisionResult
    {
        public bool intersects;
        public bool touchs;
        /// <summary>
        /// {0/0} if intersects is false
        /// </summary>
        public Vector2 location;
    }

    public class ShapeCollisionResult
    {
        public bool Intersects;
        public Vector2 PushVector;
        public double PushDistance;
        public double distTmp;
    }

}
