using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AkaArts.AgeSharp.GameProject.Main
{
    static class RectangleExtension
    {
        static Texture2D texture;

        public static void DrawBorder(this SpriteBatch batch, Rectangle rectangle, int lineWidth, Color color)
        {

            if (texture == null)
            {

                texture = new Texture2D(batch.GraphicsDevice, 1, 1);
                texture.SetData<Color>(new Color[] { Color.White });

            }

            batch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            batch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            batch.Draw(texture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            batch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);


        }
    }
}
