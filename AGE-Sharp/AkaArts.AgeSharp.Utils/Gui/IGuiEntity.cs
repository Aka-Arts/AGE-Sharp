using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils.Gui
{
    public interface IGuiEntity
    {

        public void Draw(GameTime time, SpriteBatch batch);

        public void Initialize(ContentManager content);

        public void Dispose();

    }
}
