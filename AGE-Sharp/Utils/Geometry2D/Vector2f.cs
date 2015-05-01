using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.AkaArts.AgeSharp.Utils.Geometry2D
{

    public class Vector2f
    {

        public float x;
        public float y;

        public Vector2f()
        {

            // null vector
            this.x = 0;
            this.y = 0;

        }

        public Vector2f(float x, float y)
        {

            this.x = x;
            this.y = y;

        }

        // instance methods
        public Vector2f Add(Vector2f vector)
        {

            return new Vector2f(this.x + vector.x, this.y + vector.y);

        }

        public Vector2f Subtract(Vector2f vector)
        {

            return new Vector2f(this.x - vector.x, this.y - vector.y);

        }

        public float Dot(Vector2f vector)
        {

            return (this.x * vector.x + this.y * vector.y);

        }

        public float Cross(Vector2f vector)
        {

            return (this.x * vector.y - this.y * vector.x);

        }

        public Vector2f Multiply(float scalar)
        {

            return new Vector2f(this.x * scalar, this.y * scalar);

        }


    }
}
