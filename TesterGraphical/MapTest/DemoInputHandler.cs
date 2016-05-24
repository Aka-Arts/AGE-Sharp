using AkaArts.AgeSharp.Utils.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AkaArts.AgeSharp.Utils.Commanding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TesterGraphical.MapTest
{
    class DemoInputHandler : BaseInputHandler
    {
        Keys reseed = Keys.F4;
        Keys addOct = Keys.PageUp;
        Keys remOct = Keys.PageDown;
        Keys addRough = Keys.Up;
        Keys remRough = Keys.Down;
        Keys scrollSeaLevel = Keys.A;
        Keys scrollTreeline = Keys.S;
        Keys scrollSnowline = Keys.D;

        CommandController commandController;

        public DemoInputHandler(CommandController targetCommandController) : base()
        {
            this.commandController = targetCommandController;
        }

        protected override void onUpdate(GameTime gameTime)
        {
            // reseed
            if (this.currentKeyboardState.IsKeyDown(reseed) && !this.previousKeyboardState.IsKeyDown(reseed))
            {
                this.commandController.QueueCommand("map reseed");
            }

            // octaves
            if (this.currentKeyboardState.IsKeyDown(addOct) && !this.previousKeyboardState.IsKeyDown(addOct))
            {
                this.commandController.QueueCommand("map octave add");
            }

            if (this.currentKeyboardState.IsKeyDown(remOct) && !this.previousKeyboardState.IsKeyDown(remOct))
            {
                this.commandController.QueueCommand("map octave remove");
            }

            // roughness
            if (this.currentKeyboardState.IsKeyDown(addRough) && !this.previousKeyboardState.IsKeyDown(addRough))
            {
                this.commandController.QueueCommand("map roughness add");
            }

            if (this.currentKeyboardState.IsKeyDown(remRough) && !this.previousKeyboardState.IsKeyDown(remRough))
            {
                this.commandController.QueueCommand("map roughness remove");
            }

            // wheel
            if (this.currentMouseState.ScrollWheelValue != this.previousMouseState.ScrollWheelValue)
            {

                if (this.currentKeyboardState.IsKeyDown(scrollSeaLevel))
                {
                    // sealevel
                    if (this.currentMouseState.ScrollWheelValue > this.previousMouseState.ScrollWheelValue)
                    {
                        this.commandController.QueueCommand("map sealevel lift");
                    }
                    else
                    {
                        this.commandController.QueueCommand("map sealevel lower");
                    }
                }
                else if (this.currentKeyboardState.IsKeyDown(scrollTreeline))
                {
                    // treeline
                    if (this.currentMouseState.ScrollWheelValue > this.previousMouseState.ScrollWheelValue)
                    {
                        this.commandController.QueueCommand("map treeline lift");
                    }
                    else
                    {
                        this.commandController.QueueCommand("map treeline lower");
                    }
                }
                else if (this.currentKeyboardState.IsKeyDown(scrollSnowline))
                {
                    // snowline
                    if (this.currentMouseState.ScrollWheelValue > this.previousMouseState.ScrollWheelValue)
                    {
                        this.commandController.QueueCommand("map snowline lift");
                    }
                    else
                    {
                        this.commandController.QueueCommand("map snowline lower");
                    }
                }
                else
                {
                    // zoom
                    if (this.currentMouseState.ScrollWheelValue > this.previousMouseState.ScrollWheelValue)
                    {
                        this.commandController.QueueCommand("map zoom in");
                    }
                    else
                    {
                        this.commandController.QueueCommand("map zoom out");
                    }
                }                
            }
        }
    }
}
