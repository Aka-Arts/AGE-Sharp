using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AkaArts.AgeSharp.Utils.Input
{
    public abstract class BaseInputHandler
    {
        protected KeyboardState currentKeyboardState;
        protected KeyboardState previousKeyboardState;
        protected MouseState currentMouseState;
        protected MouseState previousMouseState;

        public BaseInputHandler()
        {
            this.currentKeyboardState = Keyboard.GetState();
            this.previousKeyboardState = this.currentKeyboardState;
            this.currentMouseState = Mouse.GetState();
            this.previousMouseState = this.currentMouseState;
        }

        public void Update(GameTime time)
        {
            this.previousKeyboardState = this.currentKeyboardState;
            this.currentKeyboardState = Keyboard.GetState();
            this.previousMouseState = this.currentMouseState;
            this.currentMouseState = Mouse.GetState();
            this.onUpdate(time);
        }

        protected virtual void onUpdate(GameTime time)
        {
            // to be overridden
        }
    }
}
