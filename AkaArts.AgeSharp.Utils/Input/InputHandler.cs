using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AkaArts.AgeSharp.Utils.Input
{
    public class InputHandler
    {

        public static Keys[] AllKeysDown { get; private set; }
        public static Keys[] NewKeysDown { get; private set; }
        public static Keys[] NewKeysUp { get; private set; }

        public static Vector2 MousePosition { get; private set; }
        public static ButtonState LeftButton { get; private set; }
        public static ButtonState MiddleButton { get; private set; }
        public static ButtonState RightButton { get; private set; }
        public static ButtonState XButton1 { get; private set; }
        public static ButtonState XButton2 { get; private set; }

        private KeyboardState CurrentState;
        private KeyboardState PreviousState;

        public InputHandler()
        {

            this.CurrentState = this.PreviousState = Keyboard.GetState();
        
        }

        public void update()
        {

            // first the keyboard state

            this.CurrentState = Keyboard.GetState();

            Keys[] PreviousKeys = this.PreviousState.GetPressedKeys();
            Keys[] CurrentKeys = this.CurrentState.GetPressedKeys();

            AllKeysDown = CurrentKeys;

            NewKeysDown = CurrentKeys.Except(PreviousKeys).ToArray();
            NewKeysUp = PreviousKeys.Except(CurrentKeys).ToArray();

            this.PreviousState = this.CurrentState;

            // now the mouse state

            MouseState CurrentMouseState= Mouse.GetState();

            MousePosition = new Vector2(CurrentMouseState.X, CurrentMouseState.Y);

            LeftButton = CurrentMouseState.LeftButton;
            MiddleButton = CurrentMouseState.MiddleButton;
            RightButton = CurrentMouseState.RightButton;
            XButton1 = CurrentMouseState.XButton1;
            XButton2 = CurrentMouseState.XButton2;

        }

    }

}
