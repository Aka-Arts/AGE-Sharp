using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using AkaArts.AgeSharp.Utils.Collision;
using Microsoft.Xna.Framework;
using AkaArts.AgeSharp.Utils.Commanding;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var vert = new List<Vector2>();
            vert.Add(new Vector2(1, 1));
            vert.Add(new Vector2(-1, 1));
            vert.Add(new Vector2(-1, -1));
            vert.Add(new Vector2(1, -1));
            var poly1 = new Polygon2D(null, new Vector2(0, 0), vert);
            var poly2 = new Polygon2D(null, new Vector2(1, 0), vert);
            var move = new Vector2(-3,0);
            var result = Polygon2D.Intersect(poly1, poly2, move);

            var betterMove = move + result.PushVector;
        }
    }
}
