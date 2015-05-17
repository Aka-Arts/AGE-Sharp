using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AkaArts.AgeSharp.GameProject.Main
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static SpriteFont font;

        Ship ship;

        private Random random = new Random();

        internal static List<Asteroid> asteroids = new List<Asteroid>();

        readonly int spawnCoolDownMin = 350;

        readonly int spawnCoolDownMax = 1600;


        int nextSpawn = 0;

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

            this.ship = new Ship();
            
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

            if (asteroids.Count <= 16)
            {

                nextSpawn -= gameTime.ElapsedGameTime.Milliseconds;

                if (nextSpawn < 0)
                {
                    // spawn next asteriod

                    Asteroid.Size size = Asteroid.Size.Small;

                    if (random.Next(0,5) == 2)
                    {
                        size = Asteroid.Size.Big;
                    }
                    

                    int Y = random.Next(40, 560);

                    Asteroid newAst = new Asteroid(1050, Y, size);

                    newAst.LoadContent(Content);

                    asteroids.Add(newAst);

                    nextSpawn = random.Next(spawnCoolDownMin,spawnCoolDownMax);

                }

            }

            for (int i = 0; i < asteroids.Count ; i++)
            {
                asteroids[i].Update(gameTime);

                if (asteroids[i].Dead)
                {
                    asteroids.RemoveAt(i--);
                }

            }

            this.ship.Update(gameTime, KeyState);

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

            if (DEBUGGING)
            {
                spriteBatch.DrawString(font, "DEBUG MODE", Vector2.Zero, Color.Red);
                spriteBatch.DrawString(font, "Active Asteroids: " + asteroids.Count, new Vector2(0,20), Color.Red);
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
