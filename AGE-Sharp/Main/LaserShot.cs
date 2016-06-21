using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using AkaArts.AgeSharp.Utils.Collision;

namespace AkaArts.AgeSharp.GameProject.Main
{
    class LaserShot
    {

        Texture2D skin;

        Vector2 position;

        Vector2 origin;

        internal bool Dead = false;

        readonly float speed = 90f;

        Polygon2D collisionShape;

        public LaserShot(int x, int y)
        {

            this.position = new Vector2(x, y);

            this.origin = new Vector2(7.5f, 1.5f); // TODO origin strange behaviour?!

            this.collisionShape = Polygon2D.FromRectangle(SpaceGame.CurrentGraphicsDevice, this.position, new Rectangle(-15, -3, 30, 6));

        }

        public void LoadContent(ContentManager content)
        {

            this.skin = content.Load<Texture2D>("images/laser");

        }

        public void Update(GameTime time)
        {

            this.position.X += (speed * time.ElapsedGameTime.Milliseconds / 1000);

            this.collisionShape.Origin = this.position;

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

                if (ast.collisionShape.Intersect(this.collisionShape).Intersects)
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

                this.collisionShape.Draw(Color.Yellow);

            }

        }

    }
}
