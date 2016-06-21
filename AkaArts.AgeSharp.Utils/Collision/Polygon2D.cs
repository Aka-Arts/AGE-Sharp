using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AkaArts.AgeSharp.Utils.Collision
{
    public class Polygon2D
    {

        private Vector2 origin;

        private List<Vector2> vertices = new List<Vector2>();

        private BasicEffect effect;

        private GraphicsDevice graphics;

        public Vector2 Origin { get { return origin; } set { this.origin = value; } }

        public List<Vector2> Vertices { get { return vertices; } }

        public List<Vector2> AbsoluteVertices
        {
            get
            {
                var vertices = new List<Vector2>();

                foreach (var vertex in this.vertices)
                {
                    vertices.Add(new Vector2(vertex.X + this.origin.X, vertex.Y + this.origin.Y));
                }

                return vertices;
            }
        }

        public List<Vector2> Edges
        {
            get
            {
                var list = new List<Vector2>();

                for (int i = 0 ; i < vertices.Count ; i++)
                {
                    var vertex1 = vertices[i];

                    Vector2 vertex2;

                    if (i == vertices.Count - 1)
                    {
                        vertex2 = vertices[0];
                    }
                    else
                    {
                        vertex2 = vertices[i + 1];
                    }

                    list.Add(vertex2 - vertex1);
                }

                return list;
            }
        }

        public Polygon2D(GraphicsDevice g, Vector2 origin, List<Vector2> vertices)
        {
            this.origin = origin;
            this.vertices = vertices;
            this.graphics = g;
            if (g != null)
            {
                this.effect = new BasicEffect(g);
                this.effect.VertexColorEnabled = true;
                this.effect.Projection = Matrix.CreateOrthographicOffCenter
                (0, g.Viewport.Width,     // left, right
                g.Viewport.Height, 0,    // bottom, top
                0, 1);
            }
        }

        public void Draw(Color color)
        {
            if (this.graphics == null)
            {
                throw new InvalidOperationException("Graphics is null");
            }

            var graphicsVertices = new VertexPositionColor[vertices.Count + 1];

            for (int i = 0 ; i < vertices.Count ; i++)
            {
                graphicsVertices[i].Position = new Vector3(origin.X + vertices[i].X, origin.Y + vertices[i].Y, 0);
                graphicsVertices[i].Color = color;
            }

            graphicsVertices[vertices.Count] = graphicsVertices[0];

            this.effect.CurrentTechnique.Passes[0].Apply();
            this.graphics.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, graphicsVertices, 0, graphicsVertices.Count() - 1);
        }

        #region FACTORIES

        public static Polygon2D FromRectangle(GraphicsDevice g, Vector2 origin, Rectangle rect)
        {

            var vertices = new List<Vector2>();

            vertices.Add(new Vector2(rect.X, rect.Y));
            vertices.Add(new Vector2(rect.X, rect.Y + rect.Height));
            vertices.Add(new Vector2(rect.X + rect.Width, rect.Y + rect.Height));
            vertices.Add(new Vector2(rect.X + rect.Width, rect.Y));

            return new Polygon2D(g, origin, vertices);

        }

        #endregion

        #region COLLISION

        public ShapeCollisionResult Intersect(Polygon2D polygon2)
        {
            return Polygon2D.Intersect(this, polygon2);
        }

        public static ShapeCollisionResult Intersect(Polygon2D polygon1, Polygon2D polygon2)
        {
            var result = new ShapeCollisionResult();
            result.Intersects = true;

            var edges1 = polygon1.Edges;
            var edges2 = polygon2.Edges;

            Vector2 currentEdge;

            for (int i = 0 ; i < edges1.Count + edges2.Count ; i++)
            {
                if (i < edges1.Count)
                {
                    currentEdge = edges1[i];
                }
                else
                {
                    currentEdge = edges2[i - edges1.Count];
                }

                var currentAxis = new Vector2(-currentEdge.Y, currentEdge.X);

                currentAxis.Normalize();

                float min1 = 0;
                float max1 = 0;

                float min2 = 0;
                float max2 = 0;

                Polygon2D.ProjectOnAxis(currentAxis, polygon1, ref min1, ref max1);
                Polygon2D.ProjectOnAxis(currentAxis, polygon2, ref min2, ref max2);

                var distance = ProjectionsDistance(min1, max1, min2, max2);
                if (distance > 0)
                {
                    result.Intersects = false;
                    break; // at least one SAT was possible
                }
            }

            return result;
        }

        public static ShapeCollisionResult Intersect(Polygon2D polygon1, Polygon2D polygon2, Vector2 direction)
        {
            var result = new ShapeCollisionResult();
            result.Intersects = true;

            var edges1 = polygon1.Edges;
            var edges2 = polygon2.Edges;

            Vector2 currentEdge;

            var normalizedDirection = Vector2.Normalize(direction);
            var minDirDistance = double.MaxValue;

            for (int i = 0 ; i < edges1.Count + edges2.Count ; i++)
            {
                if (i < edges1.Count)
                {
                    currentEdge = edges1[i];
                }
                else
                {
                    currentEdge = edges2[i - edges1.Count];
                }

                var currentAxis = new Vector2(-currentEdge.Y, currentEdge.X);

                currentAxis.Normalize();

                float min1 = 0;
                float max1 = 0;

                float min2 = 0;
                float max2 = 0;

                Polygon2D.ProjectOnAxis(currentAxis, polygon1, ref min1, ref max1);
                Polygon2D.ProjectOnAxis(currentAxis, polygon2, ref min2, ref max2);

                var distance = ProjectionsDistance(min1, max1, min2, max2);
                if (distance > 0)
                {
                    result.Intersects = false;
                    break; // at least one SAT was possible
                }
                else
                {
                    // calc minDirDistance
                    var angle1 = Math.Atan2(currentAxis.Y, currentAxis.X);
                    var angle2 = Math.Atan2(normalizedDirection.Y, normalizedDirection.X);
                    var angleComp = Math.Abs(Math.Abs(angle1) - Math.Abs(angle2));

                    //if (angleComp > MathHelper.PiOver2)
                    //{
                    //    angleComp -= MathHelper.PiOver2;
                    //}

                    //var angleGrad = angleComp * (180 / Math.PI);

                    var hypot = Math.Abs(distance / Math.Cos(angleComp));
                    if (hypot < minDirDistance)
                    {
                        minDirDistance = hypot;
                    }
                }
            }

            if (result.Intersects)
            {
                result.PushVector = Vector2.Multiply(Vector2.Negate(normalizedDirection), (float) minDirDistance);
                result.PushDistance = minDirDistance;
            }
            else
            {
                result.PushVector = Vector2.Zero;
                result.PushDistance = 0;
            }

            return result;
        }

        static void ProjectOnAxis(Vector2 axis, Polygon2D polygon, ref float min, ref float max)
        {
            var vertices = polygon.AbsoluteVertices;
            var dotProduct = Vector2.Dot(axis, vertices[0]);

            min = dotProduct;
            max = dotProduct;

            for (int i = 0 ; i < vertices.Count ; i++)
            {
                dotProduct = Vector2.Dot(vertices[i], axis);

                if (dotProduct < min)
                {
                    min = dotProduct;
                }
                else if (dotProduct > max)
                {
                    max = dotProduct;
                }
            }
        }

        static float ProjectionsDistance(float min1, float max1, float min2, float max2)
        {
            if (min1 < min2)
            {
                return min2 - max1;
            }
            else
            {
                return min1 - max2;
            }
        }

        #endregion

    }
}
