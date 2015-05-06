using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AkaArts.AgeSharp.Utils.World
{
    interface IWorldEntity
    {
        /// <summary>
        /// For loading and initializing of this entity
        /// </summary>
        public void LoadContent();

        /// <summary>
        /// For Updating this entity
        /// </summary>
        /// <param name="time">Current gameTime</param>
        public void Update(GameTime time);

        /// <summary>
        /// For Drawing this entity
        /// </summary>
        /// <param name="batch">The spriteBatch for drawing</param>
        public void Draw(SpriteBatch batch);

        /// <summary>
        /// For unloadign all unmanaged assets
        /// </summary>
        public void UnloadContent();

    }
}
