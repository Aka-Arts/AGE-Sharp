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

        int windowHeight = 600;
        int windowWidth = 1000;

        int octaves = 7;
        int dimensions = 512;
        int currentSeed = 1;

        public MapTest()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;

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

            map = calcMap(currentSeed, dimensions, dimensions, octaves);

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

                currentSeed = rand.Next();

                this.map = calcMap(currentSeed, dimensions, dimensions, octaves);

            }

            if (keys.IsKeyDown(Keys.Up) && readyForRecalc)
            {
                readyForRecalc = false;

                this.map = calcMap(currentSeed, dimensions, dimensions, ++octaves);

            }

            if (keys.IsKeyDown(Keys.Down) && readyForRecalc)
            {
                readyForRecalc = false;

                octaves--;

                if (octaves < 1)
                {
                    octaves = 1;
                }

                this.map = calcMap(currentSeed, dimensions, dimensions, octaves);

            }

            if (keys.IsKeyUp(Keys.F4) &&
                keys.IsKeyUp(Keys.Up) &&
                keys.IsKeyUp(Keys.Down))
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

            spriteBatch.Draw(map, new Vector2((windowWidth / 2) - dimensions / 2, (windowHeight / 2) - dimensions / 2), Color.White);

            spriteBatch.DrawString(font, "Total calculation time: " + calcTime + " milliseconds", new Vector2(5, 5), Color.White);
            spriteBatch.DrawString(font, "Perlin: " + octaves + " octaves", new Vector2(300, 5), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Texture2D calcMap(int seed, int width, int height, int octaves)
        {

            stopwatch.Restart();

            var perlin = new PerlinMapGenerator(seed);

            var heightMap = perlin.Generate(width, height, octaves);

            var texture = new Texture2D(GraphicsDevice, width, height);

            var pixels = new Color[width * height];

            for (int i = 0 ; i < width ; i++)
            {
                for (int j = 0 ; j < height ; j++)
                {

                    pixels[(i * width) + j] = Color.Multiply(Color.Green, heightMap[i, j]);

                    if (heightMap[i,j] < 0.35)
                    {

                        pixels[(i * width) + j] = Color.Multiply(Color.Blue, heightMap[i, j]);

                    }

                    if (heightMap[i, j] > 0.68)
                    {

                        pixels[(i * width) + j] = Color.Multiply(Color.Gray, heightMap[i, j]);

                    }

                    if (heightMap[i, j] > 0.8)
                    {

                        pixels[(i * width) + j] = Color.Multiply(Color.White, heightMap[i, j]);

                    }

                    if (heightMap[(i + 1) % width,(j + 1) % height] > heightMap[i, j])
                    {
                        pixels[(i * width) + j] = Color.Lerp(pixels[(i * width) + j], Color.Black, 0.5f);
                    }

                }
            }

            texture.SetData<Color>(pixels);

            stopwatch.Stop();

            calcTime = stopwatch.ElapsedMilliseconds;

            return texture;

        }

    }
}
