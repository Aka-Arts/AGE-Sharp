using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

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

        Vector2 position;

        Vector2 origin;

        Size size;

        public bool Dead = false;

        float R = 1, G = 1, B = 1;

        readonly float fadeSpeed = 0.3f;

        readonly float minimalColor = 0.7f;

        float speed = 95f;

        bool fadeDarker = true;

        internal Rectangle AABB;

        public Asteroid(int x, int y, Size size)
        {

            this.size = size;

            this.position = new Vector2(x, y);

            switch (this.size)
            {
                case Size.Small:

                    this.AABB = new Rectangle((int)this.position.X - 28, (int)this.position.Y - 28, 54, 54);

                    this.origin = new Vector2(16,16);

                    break;

                case Size.Big:     

                    this.AABB = new Rectangle((int)this.position.X - 52, (int)this.position.Y - 58, 108, 108);

                    this.origin = new Vector2(32,32);

                    break;

            }

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

        }

        public void Update(GameTime time)
        {

            this.position.X -= (speed * time.ElapsedGameTime.Milliseconds / 1000);

            switch (this.size)
            {
                case Size.Small:

                    this.AABB = new Rectangle((int)this.position.X - 28, (int)this.position.Y - 28, 54, 54);

                    break;

                case Size.Big:

                    this.AABB = new Rectangle((int)this.position.X - 52, (int)this.position.Y - 58, 108, 108);

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

            batch.Draw(texture: this.skin, position: this.position, scale: new Vector2(2f), origin: this.origin, rotation: 0f, color: new Color(this.R, this.G, this.B));

            if (SpaceGame.DEBUGGING)
            {

                batch.DrawBorder(this.AABB, 1, Color.Red);

            }

        }

    }
}
