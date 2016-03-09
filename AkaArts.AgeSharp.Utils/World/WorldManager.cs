using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AkaArts.AgeSharp.Utils.World
{
    class WorldManager
    {

        private List<IWorldEntity> WorldEntities = new List<IWorldEntity>();
        private List<IWorldEntity> WorldEntitiesToUpdate = new List<IWorldEntity>();

        public WorldManager()
        {

        }

        public void LoadContent()
        {

        }

        public void Update(GameTime time)
        {

            WorldEntitiesToUpdate.Clear();

            foreach (IWorldEntity entity in WorldEntities)
            {
                WorldEntitiesToUpdate.Add(entity);
            }

            foreach (IWorldEntity entity in WorldEntitiesToUpdate)
            {

                entity.Update(time);

            }

        }

        public void Draw(SpriteBatch batch)
        {

            foreach (IWorldEntity entity in WorldEntities)
            {

                entity.Draw(batch);

            }

        }

        public void Add(IWorldEntity newEntity)
        {

            if (WorldEntities.Contains(newEntity))
            {
                // prevent doubles
                return;
            }

            WorldEntities.Add(newEntity);

            newEntity.LoadContent();

        }

        public void Remove(IWorldEntity newEntity)
        {

            WorldEntities.Remove(newEntity);

            newEntity.UnloadContent();

        }

    }
}
