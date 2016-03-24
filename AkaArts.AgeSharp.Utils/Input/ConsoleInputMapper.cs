using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AkaArts.AgeSharp.Utils.Commanding;
using Microsoft.Xna.Framework;
using AkaArts.AgeSharp.Utils.Console;
using Microsoft.Xna.Framework.Input;

namespace AkaArts.AgeSharp.Utils.Input
{
    internal class ConsoleInputMapper : IInputMapper
    {
        AgeConsole console;
        AgeApplication app;
        KeyboardState previousFrameKeyStates;
        KeyboardState currentFrameKeyStates;
        Keys[] previousFrameKeyDowns;
        Keys[] currentFrameKeyDowns;
        public ConsoleInputMapper(AgeConsole console, AgeApplication app)
        {
            this.console = console;
            this.app = app;
            previousFrameKeyStates = currentFrameKeyStates = Keyboard.GetState();
            previousFrameKeyDowns = currentFrameKeyDowns = new Keys[] { };
        }

        public void Update(GameTime gameTime, CommandController target)
        {
            currentFrameKeyStates = Keyboard.GetState();
            currentFrameKeyDowns = currentFrameKeyStates.GetPressedKeys();

            if (app.IsConsoleOpen)
            {
                if (AgeApplication.TextInputSinceLastFrame.Length > 0)
                {
                    console.TypeInput(AgeApplication.TextInputSinceLastFrame);
                }

                // exclusiv modifier keys
                if (currentFrameKeyStates.IsKeyDown(Keys.Delete))
                {
                    console.DeleteInput();
                }
                else if (currentFrameKeyStates.IsKeyDown(Keys.Enter) && previousFrameKeyStates.IsKeyUp(Keys.Enter))
                {
                    console.SendInput();
                }
            }

            if (currentFrameKeyStates.IsKeyDown(Keys.F12) && previousFrameKeyStates.IsKeyUp(Keys.F12))
            {
                app.ToggleConsole();
            }

            previousFrameKeyStates = currentFrameKeyStates;
            previousFrameKeyDowns = currentFrameKeyDowns;
        }
    }
}
