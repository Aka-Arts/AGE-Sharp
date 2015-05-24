using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace AkaArts.AgeSharp.Utils.Collision
{
    public static class Vector2Extension
    {

        public static float DotProduct(this Vector2 vector1, Vector2 vector2)
        {

            return (vector1.X * vector2.X) + (vector1.Y * vector2.Y);

        }

    }
}
