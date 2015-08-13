using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils.Collision
{
    public class LineSegment2D
    {

        public readonly Vector2 A, B;

        public LineSegment2D(Vector2 a, Vector2 b)
        {
            this.A = a;
            this.B = b;
        }

        public CollisionResult Intersects(LineSegment2D line)
        {
            return Intersect(this, line);
        }

        #region Static Functions

        public static CollisionResult Intersect(LineSegment2D line1, LineSegment2D line2)
        {
            var result = new CollisionResult();

            result.location = new Vector2();
            result.intersects = false;

            var r = line1.B - line1.A;
            var s = line2.B - line2.A;
            var rxs = r.Cross(s);
            var qpxr = (line2.A - line1.A).Cross(r);

            if (rxs.IsZero() && qpxr.IsZero())
            {

                if ((0 <= (line2.A - line1.A).Mult(r) && (line2.A - line1.A).Mult(r) <= r.Mult(r)) || (0 <= (line1.A - line2.A).Mult(s) && (line1.A - line2.A).Mult(s) <= s.Mult(s)))
                {

                    result.intersects = true;
                    return result;
                }
                else
                {
                    return result;
                }
            }

            if (rxs.IsZero() && !qpxr.IsZero())
            {
                return result;
            }

            var t = (line2.A - line1.A).Cross(s) / rxs;

            var u = (line2.A - line1.A).Cross(r) / rxs;

            if (!rxs.IsZero() && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                result.location = line1.A + t * r;
                result.intersects = true;
                return result;
            }
            return result;
        }

        #endregion

    }
}
