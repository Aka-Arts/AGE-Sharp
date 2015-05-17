﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AkaArts.AgeSharp.GameProject.Main
{
    class Ship
    {

        Texture2D skin;

        Texture2D explosion;

        float explosionAlpha;

        bool fadingOn;

        readonly float explosionFadeOnSpeed = 3f;
        readonly float explosionFadeOffSpeed = 1.3f;

        internal Vector2 position;

        readonly float vertical_velocity = 300f;
        readonly float forward_velocity = 250f;
        readonly float backward_velocity = 200f;
        readonly float minX = 70f;
        readonly float minY = 40f;
        readonly float maxX = 930f;
        readonly float maxY = 560f;


        Rectangle AABB1;
        Rectangle AABB2;

        bool dead = false;

        public Ship()
        {

            this.Reset();

        }

        public void LoadContent(ContentManager content)
        {

            this.skin = content.Load<Texture2D>("images/spaceship");

            this.explosion = content.Load<Texture2D>("images/explosion");

        }

        public void Update(GameTime time, KeyboardState keyStates)
        {

            this.dead = false;

            if (keyStates.IsKeyDown(Keys.W))
            {

                this.position.Y -= (vertical_velocity * time.ElapsedGameTime.Milliseconds / 1000);

                if (this.position.Y < minY)
                {
                    this.position.Y = minY;
                }

            }

            if (keyStates.IsKeyDown(Keys.S))
            {

                this.position.Y += (vertical_velocity * time.ElapsedGameTime.Milliseconds / 1000);

                if (this.position.Y > maxY)
                {
                    this.position.Y = maxY;
                }

            }

            if (keyStates.IsKeyDown(Keys.A))
            {

                this.position.X -= (backward_velocity * time.ElapsedGameTime.Milliseconds / 1000);

                if (this.position.X < minX)
                {
                    this.position.X = minX;
                }

            }

            if (keyStates.IsKeyDown(Keys.D))
            {

                this.position.X += (forward_velocity * time.ElapsedGameTime.Milliseconds / 1000);

                if (this.position.X > maxX)
                {
                    this.position.X = maxX;
                }

            }

            this.AABB1 = new Rectangle((int)this.position.X - 64, (int)this.position.Y - 28, 82, 56);
            this.AABB2 = new Rectangle((int)this.position.X, (int)this.position.Y + 10, 64, 12);

            foreach (Asteroid ast in SpaceGame.asteroids)
            {

                if (ast.AABB.Intersects(this.AABB1) || ast.AABB.Intersects(this.AABB2))
                {

                    this.dead = true;
                    SpaceGame.GameOver();

                }

            }


        }

        public void Draw(GameTime time, SpriteBatch batch)
        {
            if (this.dead)
            {
                batch.Draw(texture: this.explosion, position: this.position, scale: new Vector2(2f), origin: new Vector2(32, 32), rotation: 0f, color: new Color(1,1,1,explosionAlpha));

                if (this.fadingOn)
                {
                    this.explosionAlpha += (explosionFadeOnSpeed * time.ElapsedGameTime.Milliseconds / 1000);

                    if (this.explosionAlpha > 1)
                    {
                        this.explosionAlpha = 1;

                        this.fadingOn = false;

                    }
                }
                else
                {
                    this.explosionAlpha -= (explosionFadeOffSpeed * time.ElapsedGameTime.Milliseconds / 1000);

                    if (this.explosionAlpha < 0)
                    {
                        this.explosionAlpha = 0;
                    }
                }

                

            }
            else
            {
                batch.Draw(texture: this.skin, position: this.position, scale: new Vector2(2f), origin: new Vector2(32, 16), rotation: 0f, color: Color.White);
            }
            

            if (SpaceGame.DEBUGGING)
            {

                batch.DrawBorder(this.AABB1, 1, Color.Lime);

                batch.DrawBorder(this.AABB2, 1, Color.Lime);

            }


        }

        public void Reset()
        {

            this.position = new Vector2(100, 300);

            this.explosionAlpha = 0;

            this.fadingOn = true;

        }

    }
}
