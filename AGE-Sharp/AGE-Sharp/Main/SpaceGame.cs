using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AkaArts.AgeSharp.Utils.Collision;
using AkaArts.AgeSharp.Utils;

namespace AkaArts.AgeSharp.GameProject.Main
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static GraphicsDevice CurrentGraphicsDevice;

        public static SpriteFont font;
        Ship ship;

        Texture2D LaserCharge;

        SoundEffect laserShot;

        readonly int laserCoolDown = 300;

        int nextLaser = 0;

        readonly int ChargeCoolDown = 3000;

        int nextCharge = 0;

        private Random random = new Random();

        readonly int maxShots = 3;

        int shots;

        internal static List<Asteroid> asteroids = new List<Asteroid>();

        internal static List<LaserShot> laserShots = new List<LaserShot>();

        readonly int spawnCoolDownMin = 350;
        readonly int spawnCoolDownMax = 1200;

        int nextSpawn = 0;

        static bool over = false;

        // debugging

        public static bool DEBUGGING = false;


        public SpaceGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 1000;


            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            CurrentGraphicsDevice = GraphicsDevice;

            this.ship = new Ship();

            Reset();
            
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("fonts/Nulshock-14-regular");

            LaserCharge = Content.Load<Texture2D>("images/laser");

            laserShot = Content.Load<SoundEffect>("sounds/lasergun");

            ship.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            KeyboardState KeyState = Keyboard.GetState();

            if (KeyState.IsKeyDown(Keys.Escape))
            {

                Exit();
            
            }

            if (KeyState.IsKeyDown(Keys.End))
            {
                DEBUGGING = true;
            }


            nextSpawn -= gameTime.ElapsedGameTime.Milliseconds;

            if (nextSpawn < 0)
            {
                // spawn next asteriod

                Asteroid.Size size = Asteroid.Size.Small;

                if (random.Next(0, 5) == 2)
                {
                    size = Asteroid.Size.Big;
                }


                int Y = random.Next(40, 560);

                Asteroid newAst = new Asteroid(1050, Y, size);

                newAst.LoadContent(Content);

                asteroids.Add(newAst);

                nextSpawn = random.Next(spawnCoolDownMin, spawnCoolDownMax);

            }


            for (int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].Update(gameTime);

                if (asteroids[i].Dead)
                {
                    asteroids.RemoveAt(i--);
                }

            }

            if (!over)
            {

                if (KeyState.IsKeyDown(Keys.Space) && nextLaser == 0 && shots > 0)
                {
                    shots--;

                    nextLaser = laserCoolDown;

                    laserShot.TryPlay();

                    LaserShot newShot = new LaserShot((int)ship.position.X + 38, (int)ship.position.Y + 28);

                    newShot.LoadContent(Content);

                    laserShots.Add(newShot);

                }

                for (int i = 0; i < laserShots.Count; i++)
                {
                    laserShots[i].Update(gameTime);

                    if (laserShots[i].Dead)
                    {
                        laserShots.RemoveAt(i--);
                    }

                }

                this.ship.Update(gameTime, KeyState);

                nextLaser -= gameTime.ElapsedGameTime.Milliseconds;

                if (nextLaser < 0)
                {
                    nextLaser = 0;
                }


                // recharge

                if (shots < maxShots)
                {

                    nextCharge -= gameTime.ElapsedGameTime.Milliseconds;

                    if (nextCharge < 0)
                    {
                        shots++;
                        nextCharge = ChargeCoolDown;
                    }


                }


            }
            else
            {

                if (KeyState.IsKeyDown(Keys.Enter))
                {
                    Reset();
                }

            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(10,10,10));

            // TODO: Add your drawing code here
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.NonPremultiplied);

            ship.Draw(gameTime, spriteBatch);

            foreach (var ast in asteroids)
            {
                ast.Draw(gameTime, spriteBatch);
            }

            foreach (var laser in laserShots)
            {
                laser.Draw(gameTime, spriteBatch);
            }

            Vector2 shot1 = new Vector2(0, 6);
            Vector2 shot2 = new Vector2(0, 14);
            Vector2 shot3 = new Vector2(0, 22);

            switch (shots)
            {
                case 3:

                    spriteBatch.Draw(texture: LaserCharge, position: shot1, color: Color.White, scale: new Vector2(2f));

                    spriteBatch.Draw(texture: LaserCharge, position: shot2, color: Color.White, scale: new Vector2(2f));

                    spriteBatch.Draw(texture: LaserCharge, position: shot3, color: Color.White, scale: new Vector2(2f));

                    break;
                case 2:

                    spriteBatch.Draw(texture: LaserCharge, position: shot1, color: Color.White, scale: new Vector2(2f));

                    spriteBatch.Draw(texture: LaserCharge, position: shot2, color: Color.White, scale: new Vector2(2f));

                    spriteBatch.Draw(texture: LaserCharge, position: shot3, color: Color.DarkGray, scale: new Vector2(2f));

                    break;
                case 1:

                    spriteBatch.Draw(texture: LaserCharge, position: shot1, color: Color.White, scale: new Vector2(2f));

                    spriteBatch.Draw(texture: LaserCharge, position: shot2, color: Color.DarkGray, scale: new Vector2(2f));

                    spriteBatch.Draw(texture: LaserCharge, position: shot3, color: Color.DarkGray, scale: new Vector2(2f));

                    break;
                default:

                    spriteBatch.Draw(texture: LaserCharge, position: shot1, color: Color.DarkGray, scale: new Vector2(2f));

                    spriteBatch.Draw(texture: LaserCharge, position: shot2, color: Color.DarkGray, scale: new Vector2(2f));

                    spriteBatch.Draw(texture: LaserCharge, position: shot3, color: Color.DarkGray, scale: new Vector2(2f));

                    break;
            }

            if (over)
            {

                spriteBatch.DrawString(font, "GAME OVER", new Vector2(500, 300), Color.LightGray, 0f, new Vector2(80,12),2f, SpriteEffects.None, 1);
                spriteBatch.DrawString(font, "Press ENTER to play again!", new Vector2(500, 350), Color.LightSteelBlue, 0f, new Vector2(190, 12), 1f, SpriteEffects.None, 1);

            }

            if (DEBUGGING)
            {
                spriteBatch.DrawString(font, "DEBUG MODE", new Vector2(50, 0), Color.Red);
                spriteBatch.DrawString(font, "Active Asteroids: " + asteroids.Count, new Vector2(50, 16), Color.Red);
                spriteBatch.DrawString(font, "Active Lasers: " + laserShots.Count, new Vector2(50, 32), Color.Red);
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void GameOver(){

            over = true;

            laserShots.Clear();

        }

        private void Reset()
        {

            over = false;
            asteroids.Clear();

            shots = maxShots;

            nextCharge = ChargeCoolDown;

            ship.Reset();

        }

    }
}
