using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils.Gui
{
    public class Gui
    {

        private Dictionary<string, IGuiEntity> GuiElementsByID;

        public Gui()
        {
            GuiElementsByID = new Dictionary<string, IGuiEntity>();
        }



    }
}
