using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AkaArts.AgeSharp.Utils.Collision;
using Microsoft.Xna.Framework;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {

            var line1 = new LineSegment2D(new Vector2(0, 0), new Vector2(0, 5));
            var line2 = new LineSegment2D(new Vector2(2, 2), new Vector2(2, 5));

            var result = LineSegment2D.Intersect(line1,line2);

            Console.Write("intersection: " + result.intersects + " at " + result.location);

            Console.ReadLine();

            return;
        }
    }
}
