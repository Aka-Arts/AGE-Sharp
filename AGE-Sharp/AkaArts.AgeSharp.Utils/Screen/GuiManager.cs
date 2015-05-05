using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AkaArts.AgeSharp.Utils.Screen
{
    public class GuiManager
    {

        private List<Gui> AllGuis = new List<Gui>();
        private List<Gui> ActiveGuis = new List<Gui>();
        private List<Gui> GuisToUpdate = new List<Gui>();

        public GuiManager()
        {

        }

        public void LoadContent()
        {

        }

        public void Update(GameTime time)
        {

            ActiveGuis.Clear();
            GuisToUpdate.Clear();

            foreach (Gui gui in AllGuis)
            {
                GuisToUpdate.Add(gui);
            }

            foreach (Gui gui in GuisToUpdate)
            {

                gui.Update(time);

                if (gui.IsActive)
                {
                    ActiveGuis.Add(gui);
                }

            }

        }

        public void Draw(SpriteBatch batch)
        {

            foreach (Gui gui in ActiveGuis)
            {

                gui.Draw(batch);

            }

        }

        public void Add(Gui newGui)
        {

            if (AllGuis.Contains(newGui))
            {
                // prevent doubles
                return;
            }

            AllGuis.Add(newGui);

            newGui.LoadContent();

        }

        public void Remove(Gui gui)
        {

            AllGuis.Remove(gui);

            gui.UnloadContent();

        }

    }
}
