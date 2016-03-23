using AkaArts.AgeSharp.Utils.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AkaArts.AgeSharp.Utils.Commanding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TesterGraphical
{
    class DemoInputMapper : IInputMapper
    {
        Keys reseed = Keys.F4;
        Keys addOct = Keys.PageUp;
        Keys remOct = Keys.PageDown;
        Keys addRough = Keys.Up;
        Keys remRough = Keys.Down;
        Keys scrollSeaLevel = Keys.A;
        Keys scrollTreeline = Keys.S;
        Keys scrollSnowline = Keys.D;

        KeyboardState previousFrameKeyboard;
        MouseState previousFrameMouse;

        public DemoInputMapper()
        {
            previousFrameKeyboard = Keyboard.GetState();
            previousFrameMouse = Mouse.GetState();
        }

        public void Update(GameTime gameTime, CommandController target)
        {
            var currentFrameKeyboard = Keyboard.GetState();
            var currentFrameMouse = Mouse.GetState();

            // reseed
            if (currentFrameKeyboard.IsKeyDown(reseed) && !previousFrameKeyboard.IsKeyDown(reseed))
            {
                target.QueueCommand("map reseed");
            }

            // octaves
            if (currentFrameKeyboard.IsKeyDown(addOct) && !previousFrameKeyboard.IsKeyDown(addOct))
            {
                target.QueueCommand("map octave add");
            }

            if (currentFrameKeyboard.IsKeyDown(remOct) && !previousFrameKeyboard.IsKeyDown(remOct))
            {
                target.QueueCommand("map octave remove");
            }

            // roughness
            if (currentFrameKeyboard.IsKeyDown(addRough) && !previousFrameKeyboard.IsKeyDown(addRough))
            {
                target.QueueCommand("map roughness add");
            }

            if (currentFrameKeyboard.IsKeyDown(remRough) && !previousFrameKeyboard.IsKeyDown(remRough))
            {
                target.QueueCommand("map roughness remove");
            }

            // wheel
            if (currentFrameMouse.ScrollWheelValue != previousFrameMouse.ScrollWheelValue)
            {

                if (currentFrameKeyboard.IsKeyDown(scrollSeaLevel))
                {
                    // sealevel
                    if (currentFrameMouse.ScrollWheelValue > previousFrameMouse.ScrollWheelValue)
                    {
                        target.QueueCommand("map sealevel lift");
                    }
                    else
                    {
                        target.QueueCommand("map sealevel lower");
                    }
                }
                else if (currentFrameKeyboard.IsKeyDown(scrollTreeline))
                {
                    // treeline
                    if (currentFrameMouse.ScrollWheelValue > previousFrameMouse.ScrollWheelValue)
                    {
                        target.QueueCommand("map treeline lift");
                    }
                    else
                    {
                        target.QueueCommand("map treeline lower");
                    }
                }
                else if (currentFrameKeyboard.IsKeyDown(scrollSnowline))
                {
                    // snowline
                    if (currentFrameMouse.ScrollWheelValue > previousFrameMouse.ScrollWheelValue)
                    {
                        target.QueueCommand("map snowline lift");
                    }
                    else
                    {
                        target.QueueCommand("map snowline lower");
                    }
                }
                else
                {
                    // zoom
                    if (currentFrameMouse.ScrollWheelValue > previousFrameMouse.ScrollWheelValue)
                    {
                        target.QueueCommand("map zoom in");
                    }
                    else
                    {
                        target.QueueCommand("map zoom out");
                    }
                }                
            }
            previousFrameKeyboard = currentFrameKeyboard;
            previousFrameMouse = currentFrameMouse;
        }
    }
}
