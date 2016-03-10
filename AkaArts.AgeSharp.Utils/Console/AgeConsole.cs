using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkaArts.AgeSharp.Utils.Console
{
    public class AgeConsole
    {
        private AgeApplication app;

        private String currentInputBuffer = "";

        private int lineBufferCapacity = 5;
        private Queue<String> lineBuffer;

        private Texture2D consoleBackground;
        private Rectangle consoleBackgroundRect;
        private int consoleBackgroundHeight = 150;

        internal AgeConsole(AgeApplication app)
        {
            this.app = app;
            this.lineBuffer = new Queue<String>(lineBufferCapacity);

            this.consoleBackground = new Texture2D(app.GraphicsDevice, 1, 1);
            this.consoleBackground.SetData<Color>(new Color[] { new Color(0.2f, 0.2f, 0.2f, 0.3f) });
            this.consoleBackgroundRect = new Rectangle(0, app.WindowHeight - consoleBackgroundHeight, app.WindowWidth, consoleBackgroundHeight);
        }

        internal void Update(GameTime gameTime)
        {
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(consoleBackground, consoleBackgroundRect, Color.White);

            spriteBatch.End();
        }

        internal void Type(String str)
        {
            currentInputBuffer += str;
        }

        internal void DeleteInputToLeft()
        {
            if (currentInputBuffer.Length > 0)
            {
                currentInputBuffer = currentInputBuffer.Substring(0, currentInputBuffer.Length - 1);
            }
        }

        internal void DeleteInput()
        {
            currentInputBuffer = "";
        }

        internal void WriteLine(String str)
        {
            while (lineBuffer.Count >= 5)
            {
                lineBuffer.Dequeue();
            }
            lineBuffer.Enqueue(str);
        }
    }
}
