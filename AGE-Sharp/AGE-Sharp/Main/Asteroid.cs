using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using AkaArts.AgeSharp.Utils.Collision;
using AkaArts.AgeSharp.Utils;

namespace AkaArts.AgeSharp.GameProject.Main
{
    class Asteroid
    {

        public enum Size
        {
            Small,
            Big,
        }

        Texture2D skin;

        Texture2D explosion;

        SoundEffect explosionEffect;

        Vector2 position;

        Vector2 origin;

        Size size;

        float explosionAlpha;

        bool fadingOn;

        readonly float explosionFadeOnSpeed = 4f;
        readonly float explosionFadeOffSpeed = 1.8f;

        internal bool Dead = false;

        internal bool hit = false;

        float R = 1, G = 1, B = 1;

        readonly float fadeSpeed = 0.3f;

        readonly float minimalColor = 0.7f;

        float speed = 95f;

        static Random random = new Random();

        float rotation;

        bool fadeDarker = true;

        //internal Rectangle AABB;

        internal Polygon2D collisionShape;

        public Asteroid(int x, int y, Size size)
        {

            this.size = size;

            this.position = new Vector2(x, y);

            this.rotation = (random.Next(0,90)- 45) * ((float)Math.PI / 180f);

            switch (this.size)
            {
                case Size.Small:

                    //this.AABB = new Rectangle((int)this.position.X - 28, (int)this.position.Y - 28, 54, 54);

                    this.collisionShape = Polygon2D.FromRectangle(SpaceGame.CurrentGraphicsDevice, this.position, new Rectangle(-28, -28, 54, 54));

                    this.origin = new Vector2(16,16);

                    break;

                case Size.Big:     

                    //this.AABB = new Rectangle((int)this.position.X - 52, (int)this.position.Y - 58, 108, 108);

                    this.collisionShape = Polygon2D.FromRectangle(SpaceGame.CurrentGraphicsDevice, this.position, new Rectangle(-52, -58, 108, 108));


                    this.origin = new Vector2(32,32);

                    break;

            }

            this.explosionAlpha = 0;

            this.fadingOn = true;

        }

        public void LoadContent(ContentManager content)
        {

            switch (this.size)
            {
                case Size.Small:

                    this.skin = content.Load<Texture2D>("images/asteroid_small");

                    break;
                case Size.Big:

                    this.skin = content.Load<Texture2D>("images/asteroid_big");

                    break;
            }

            this.explosion = content.Load<Texture2D>("images/explosion");

            this.explosionEffect = content.Load<SoundEffect>("sounds/explosion");

        }

        public void Update(GameTime time)
        {

            this.position.X -= (speed * time.ElapsedGameTime.Milliseconds / 1000);

            switch (this.size)
            {
                case Size.Small:

                    //this.AABB = new Rectangle((int)this.position.X - 28, (int)this.position.Y - 28, 54, 54);

                    //this.AABB.X = (int)this.position.X - 28;

                    this.collisionShape.Origin = this.position;

                    break;

                case Size.Big:

                    //this.AABB.X = (int)this.position.X - 52;

                    this.collisionShape.Origin = this.position;

                    break;

            }

            if (this.position.X < -50f)
            {
                Dead = true;
            }

            #region fading

            float fade = (fadeSpeed * time.ElapsedGameTime.Milliseconds / 1000);

            switch (this.size)
            {
                case Size.Small:

                    if (fadeDarker)
                    {

                        this.R -= fade;
                        this.B -= fade;

                        if (this.R < minimalColor || this.B < minimalColor)
                        {
                            this.R = minimalColor;
                            this.B = minimalColor;

                            fadeDarker = false;
                        }

                    }
                    else
                    {

                        this.R += fade;
                        this.B += fade;

                        if (this.R > 1 || this.B > 1)
                        {
                            this.R = 1;
                            this.B = 1;

                            fadeDarker = true;
                        }

                    }


                    break;
                case Size.Big:

                    if (fadeDarker)
                    {

                        this.G -= fade;
                        this.B -= fade;

                        if (this.G < minimalColor || this.B < minimalColor)
                        {
                            this.G = minimalColor;
                            this.B = minimalColor;

                            fadeDarker = false;
                        }

                    }
                    else
                    {

                        this.G += fade;
                        this.B += fade;

                        if (this.G > 1 || this.B > 1)
                        {
                            this.G = 1;
                            this.B = 1;

                            fadeDarker = true;
                        }

                    }

                    break;
            }

            #endregion

        }

        public void Draw(GameTime time, SpriteBatch batch)
        {

            if (this.hit)
            {
                batch.Draw(texture: this.explosion, position: this.position, scale: new Vector2(2f), origin: new Vector2(32, 32), rotation: this.rotation, color: new Color(1, 1, 1, explosionAlpha));

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
                        this.Dead = true;
                    }
                }



            }
            else
            {
                batch.Draw(texture: this.skin, position: this.position, scale: new Vector2(2f), origin: this.origin, rotation: this.rotation, color: new Color(this.R, this.G, this.B));

                if (SpaceGame.DEBUGGING)
                {

                    //batch.DrawBorder(this.AABB, 1, Color.Red);

                    this.collisionShape.Draw(Color.Red);

                }

            }


            

        }

        public void Hit()
        {

            float pitch;

            switch (this.size)
            {
                case Size.Big:
                    pitch = -0.4f;
                    break;
                case Size.Small:
                default:
                    pitch = -0.1f;
                    break;
            }

            this.explosionEffect.TryPlay(1, pitch, 0);

            this.hit = true;

        }

    }
}
