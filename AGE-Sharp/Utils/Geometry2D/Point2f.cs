using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.AkaArts.AgeSharp.Utils.Geometry2D
{

    public class Point2f
    {

        public float x;
        public float y;

        public Point2f()
        {

            this.x = 0;
            this.y = 0;

        }

        public Point2f(float x, float y)
        {

            this.x = x;
            this.y = y;

        }

        // instance methods
        public Point2f Add(Vector2f vector)
        {

            return new Point2f(this.x + vector.x, this.y + vector.y);

        }

        public Point2f Subtract(Vector2f vector)
        {

            return new Point2f(this.x - vector.x, this.y - vector.y);

        }

        public Vector2f Subtract(Point2f point)
        {

            return new Vector2f(this.x - point.x, this.y - point.y);

        }

    }

}
