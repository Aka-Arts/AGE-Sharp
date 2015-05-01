using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.AkaArts.AgeSharp.Utils.Geometry2D
{

    public class LineSegment4f
    {

        public Point2f P1;
        public Point2f P2;

        public LineSegment4f()
        {

            this.P1 = new Point2f();
            this.P2 = new Point2f();

        }

        public LineSegment4f(Point2f point1, Point2f point2)
        {

            this.P1 = point1;
            this.P2 = point2;

        }

        public Point2f Intersect(LineSegment4f line){

            Vector2f line1P1ToP2 = this.P2.Subtract(this.P1);
            Vector2f line2P1ToP2 = line.P2.Subtract(line.P1);

            float line1VectorCrossLine2Vector = line1P1ToP2.Cross(line2P1ToP2);

            Vector2f line2P1MinusLine1P1 = line.P1.Subtract(this.P1);

            float scalar1 =
                line2P1MinusLine1P1
                .Cross(line2P1ToP2)
                / line1VectorCrossLine2Vector;

            float scalar2 =
                line2P1MinusLine1P1
                .Cross(line1P1ToP2)
                / line1VectorCrossLine2Vector;

            if (line1VectorCrossLine2Vector != 0)
            {

                if ((0 <= scalar1 && scalar1 <= 1) &&
                    (0 <= scalar2 && scalar2 <= 1))
                {

                    Point2f intersection = this.P1.Add(line1P1ToP2.Multiply(scalar1));

                    return intersection;

                }

            }

            return null;

        }

    }
}
