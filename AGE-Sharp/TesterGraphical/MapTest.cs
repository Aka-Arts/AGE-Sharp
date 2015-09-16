using AkaArts.AgeSharp.Utils.Graphics;
using AkaArts.AgeSharp.Utils.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TesterGraphical
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MapTest : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;

        long calcTime = 0;

        bool readyForRecalc = true;

        Random rand = new Random();

        Texture2D map;

        Stopwatch stopwatch = new Stopwatch();

        public MapTest()
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

            this.Window.Title = "perlin map tests benchmark";

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

            font = Content.Load<SpriteFont>("fonts/Arial-12-regular");

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.F4) && readyForRecalc)
            {
                readyForRecalc = false;

                this.map = calcMap(1, 512, 512, 5);

            }

            if (keys.IsKeyUp(Keys.F4))
            {
                readyForRecalc = true;    
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

                spriteBatch.Draw(map, new Vector2(50, 50), Color.White);

                spriteBatch.DrawString(font, "Total calculation time: " + calcTime + " milliseconds", new Vector2(5, 5), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Texture2D calcMap(int seed, int width, int height, int octaves)
        {

            stopwatch.Restart();

            var perlin = new PerlinMapGenerator(seed);

            var heightMap = perlin.Generate(width, height, octaves);

            var texture = new Texture2D(GraphicsDevice, width, height);

            //var pixels = new 

            //for (int i = 0 ; i < width ; i++)
            //{
            //    for (int j = 0 ; j < height ; j++)
            //    {
                    
            //        texture.SetData<>

            //    }
            //}

            stopwatch.Stop();

            calcTime = stopwatch.ElapsedMilliseconds;

            return texture;

        }

    }
}
