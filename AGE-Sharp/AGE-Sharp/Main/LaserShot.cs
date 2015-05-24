using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AkaArts.AgeSharp.GameProject.Main
{
    class LaserShot
    {

        Texture2D skin;

        Vector2 position;

        Vector2 origin;

        internal bool Dead = false;

        readonly float speed = 900f;

        Rectangle AABB;

        public LaserShot(int x, int y)
        {

            this.position = new Vector2(x, y);

            this.AABB = new Rectangle((int)this.position.X - 6, (int)this.position.Y - 6, 30, 6);

            this.origin = new Vector2(15, 3);

        }

        public void LoadContent(ContentManager content)
        {

            this.skin = content.Load<Texture2D>("images/laser");

        }

        public void Update(GameTime time)
        {

            this.position.X += (speed * time.ElapsedGameTime.Milliseconds / 1000);

            this.AABB.X = (int)this.position.X - 28;

            if (this.position.X > 1000)
            {
                Dead = true;

                return;
            }

            foreach (Asteroid ast in SpaceGame.asteroids)
            {

                if (ast.hit)
                {
                    continue;
                }

                if (ast.AABB.Intersects(this.AABB))
                {

                    this.Dead = true;
                    ast.Hit();

                }

            }

        }

        public void Draw(GameTime time, SpriteBatch batch)
        {

            batch.Draw(texture: this.skin, position: this.position, scale: new Vector2(2f), origin: this.origin, rotation: 0f, color: Color.White);

            if (SpaceGame.DEBUGGING)
            {

                batch.DrawBorder(this.AABB, 1, Color.Red);

            }

        }

    }
}
