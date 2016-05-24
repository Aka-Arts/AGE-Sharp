using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AkaArts.AgeSharp.Utils.Commanding;
using Microsoft.Xna.Framework;
using AkaArts.AgeSharp.Utils.Console;
using Microsoft.Xna.Framework.Input;
using AkaArts.AgeSharp.Utils.Input;
using AkaArts.AgeSharp.Utils;

namespace AkaArts.AgeSharp.Utils.Console
{
    internal class ConsoleInputHandler : BaseInputHandler
    {
        private AgeConsole console;
        private AgeApplication app;

        public ConsoleInputHandler(AgeConsole console, AgeApplication app) : base()
        {
            this.console = console;
            this.app = app;
        }

        protected override void onUpdate(GameTime gameTime)
        {
            if (app.IsConsoleOpen)
            {
                if (AgeApplication.TextInputSinceLastFrame.Length > 0)
                {
                    console.TypeInput(AgeApplication.TextInputSinceLastFrame);
                }

                // exclusiv modifier keys
                if (this.currentKeyboardState.IsKeyDown(Keys.Delete))
                {
                    console.DeleteInput();
                }
                else if (this.currentKeyboardState.IsKeyDown(Keys.Back))
                {
                    console.DeleteInputToLeft();
                }
                else if (this.currentKeyboardState.IsKeyDown(Keys.Enter) && this.previousKeyboardState.IsKeyUp(Keys.Enter))
                {
                    console.SendInput();
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.F12) && this.previousKeyboardState.IsKeyUp(Keys.F12))
            {
                app.ToggleConsole();
            }
        }
    }
}
