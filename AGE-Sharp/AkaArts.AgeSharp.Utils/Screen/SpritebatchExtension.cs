using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AkaArts.AgeSharp.Utils.Screen
{
    public static class SpritebatchExtension
    {

        public static void DrawOutlinedString(this SpriteBatch batch, SpriteFont font, String text, Vector2 position, Color color, Color outlineColor, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            
            // horizontal/vertikal
            batch.DrawString(font, text, new Vector2(position.X, position.Y - 1), outlineColor, rotation, origin, scale, effects, layerDepth);
            batch.DrawString(font, text, new Vector2(position.X, position.Y + 1), outlineColor, rotation, origin, scale, effects, layerDepth);
            batch.DrawString(font, text, new Vector2(position.X + 1, position.Y), outlineColor, rotation, origin, scale, effects, layerDepth);
            batch.DrawString(font, text, new Vector2(position.X - 1, position.Y), outlineColor, rotation, origin, scale, effects, layerDepth);


            // diagonal
            batch.DrawString(font, text, new Vector2(position.X - 1, position.Y - 1), outlineColor, rotation, origin, scale, effects, layerDepth);
            batch.DrawString(font, text, new Vector2(position.X - 1, position.Y + 1), outlineColor, rotation, origin, scale, effects, layerDepth);
            batch.DrawString(font, text, new Vector2(position.X + 1, position.Y + 1), outlineColor, rotation, origin, scale, effects, layerDepth);
            batch.DrawString(font, text, new Vector2(position.X + 1, position.Y - 1), outlineColor, rotation, origin, scale, effects, layerDepth);

            batch.DrawString(font, text, position, color, rotation, origin, scale, effects, layerDepth);

        }

    }
}
