﻿using AkaArts.AgeSharp.Utils.Content;
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

        private const int lineBufferCapacity = 5;
        private List<String> lineBuffer;

        private Texture2D consoleBackground;
        private Rectangle consoleBackgroundRect;
        private Vector2 consoleCurrentInputPos;
        private int consoleBackgroundHeight = 150;
        private int lineHeight = 20;
        private int offsetLeft = 20;

        private DateTime lastDeleteToLeft = DateTime.Now;

        internal AgeConsole(AgeApplication app)
        {
            this.app = app;
            this.lineBuffer = new List<String>(lineBufferCapacity);

            this.consoleBackground = new Texture2D(app.GraphicsDevice, 1, 1);
            this.consoleBackground.SetData<Color>(new Color[] { new Color(0.2f, 0.2f, 0.2f, 0.3f) });
            this.consoleBackgroundRect = new Rectangle(0, app.WindowHeight - consoleBackgroundHeight, app.WindowWidth, consoleBackgroundHeight);
            this.consoleCurrentInputPos = new Vector2(offsetLeft, consoleBackgroundRect.Y + (lineBufferCapacity * lineHeight) + lineHeight);
        }

        internal void Update(GameTime gameTime)
        {
        }

        internal void Draw(GameTime gameTime)
        {
            app.SpriteBatch.Begin();
            app.SpriteBatch.Draw(consoleBackground, consoleBackgroundRect, Color.White);

            var usedLines = lineBuffer.Count;
            for (int i = 0 ; i < usedLines ; i++)
            {
                var lineOffsetY = (i * lineHeight) + ((lineBufferCapacity - usedLines) * lineHeight) + lineHeight;
                var currentLine = lineBuffer[i];
                var lineVec = new Vector2(offsetLeft, consoleBackgroundRect.Y + lineOffsetY);

                app.SpriteBatch.DrawString(AgeDefaultContent.FONT, currentLine, lineVec, Color.White);
            }

            app.SpriteBatch.DrawString(AgeDefaultContent.FONT, ">" + currentInputBuffer, consoleCurrentInputPos, Color.White);
            app.SpriteBatch.End();
        }

        internal void TypeInput(String str)
        {
            currentInputBuffer += str;
        }

        internal void DeleteInputToLeft()
        {
            var now = DateTime.Now;
            if (currentInputBuffer.Length > 0 &&
                (now - lastDeleteToLeft).Milliseconds > 80)
            {
                currentInputBuffer = currentInputBuffer.Substring(0, currentInputBuffer.Length - 1);
                lastDeleteToLeft = now;
            }
        }

        internal void DeleteInput()
        {
            currentInputBuffer = "";
        }

        internal void SendInput()
        {
            WriteLine(">" + currentInputBuffer);
            app.CommandController.QueueCommand(currentInputBuffer);
            currentInputBuffer = "";
        }

        public void WriteLine(String str)
        {
            while (lineBuffer.Count >= 5)
            {
                lineBuffer.RemoveAt(0);
            }
            lineBuffer.Add(str);
        }
    }
}
