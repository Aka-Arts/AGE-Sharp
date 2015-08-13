using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AkaArts.AgeSharp.Utils.Collision
{
    public class PrimitiveDrawer
    {

        private BasicEffect effect;
        private GraphicsDevice graphics;

        public PrimitiveDrawer(GraphicsDevice g)
        {
            this.graphics = g;

            this.effect = new BasicEffect(g);
            this.effect.VertexColorEnabled = true;
            this.effect.Projection = Matrix.CreateOrthographicOffCenter
            (0, g.Viewport.Width,     // left, right
            g.Viewport.Height, 0,    // bottom, top
            0, 1);

        }

        public void DrawLine(LineSegment2D line, Color color)
        {

            var graphicsVertices = new VertexPositionColor[2];

            
            graphicsVertices[0].Position = new Vector3(line.A.X, line.A.Y, 0);
            graphicsVertices[0].Color = color;

            graphicsVertices[1].Position = new Vector3(line.B.X, line.B.Y, 0);
            graphicsVertices[1].Color = color;

            this.effect.CurrentTechnique.Passes[0].Apply();

            this.graphics.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, graphicsVertices, 0, 1);

        }

    }
}
