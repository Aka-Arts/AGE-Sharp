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

        float scale = 0.002f;
        float zoom = 0.0005f;
        float roughness = 0.5f;
        float roughnessSteps = 0.025f;

        int wheelValue = 0;

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

            map = calcMap(currentSeed, dimensions, dimensions, octaves, roughness, scale);

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

            bool doRecalc = false;

            if (IsActive)
            {
                var keys = Keyboard.GetState();

                var mouse = Mouse.GetState();

                if (keys.IsKeyDown(Keys.F4) && readyForRecalc)
                {
                    readyForRecalc = false;

                    currentSeed = rand.Next();

                    doRecalc = true;

                }

                if (keys.IsKeyDown(Keys.PageUp) && readyForRecalc)
                {
                    readyForRecalc = false;

                    doRecalc = true;

                    octaves++;
                }

                if (keys.IsKeyDown(Keys.PageDown) && readyForRecalc)
                {
                    readyForRecalc = false;

                    octaves--;

                    if (octaves < 1)
                    {
                        octaves = 1;
                    }

                    doRecalc = true;
                }

                if (keys.IsKeyDown(Keys.Up) && readyForRecalc)
                {
                    readyForRecalc = false;

                    doRecalc = true;

                    roughness += roughnessSteps;

                    if (roughness > 1)
                    {
                        roughness = 1;
                    }

                }

                if (keys.IsKeyDown(Keys.Down) && readyForRecalc)
                {
                    readyForRecalc = false;

                    roughness -= roughnessSteps;

                    if (roughness < 0)
                    {
                        roughness = 0;
                    }

                    doRecalc = true;
                }

                if (mouse.ScrollWheelValue != wheelValue)
                {

                    if (mouse.ScrollWheelValue > wheelValue)
                    {
                        scale += zoom;
                    }
                    else
                    {
                        scale -= zoom;
                        if (scale < zoom)
                        {
                            scale = zoom;
                        }
                    }

                    wheelValue = mouse.ScrollWheelValue;

                    doRecalc = true;
                }


                if (keys.IsKeyUp(Keys.F4) &&
                    keys.IsKeyUp(Keys.Up) &&
                    keys.IsKeyUp(Keys.Down))
                {
                    readyForRecalc = true;
                }

            }

            if (doRecalc)
            {
                this.map = calcMap(currentSeed, dimensions, dimensions, octaves, roughness, scale);
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
            spriteBatch.DrawString(font, "Simplex: " + octaves + " octaves", new Vector2(300, 5), Color.White);
            spriteBatch.DrawString(font, "Roughness: " + roughness, new Vector2(5, 25), Color.White);
            spriteBatch.DrawString(font, "Zoom: " + scale, new Vector2(300, 25), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Texture2D calcMap(int seed, int width, int height, int octaves, float roughness, float scale)
        {

            stopwatch.Restart();

            var simplex = new SimplexMapGenerator();

            var heightMap = simplex.Generate(width, height, octaves, roughness, scale);

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

                    //if (heightMap[(i + 1) % width,(j + 1) % height] > heightMap[i, j])
                    //{
                    //    pixels[(i * width) + j] = Color.Lerp(pixels[(i * width) + j], Color.Black, 0.5f);
                    //}

                }
            }

            texture.SetData<Color>(pixels);

            stopwatch.Stop();

            calcTime = stopwatch.ElapsedMilliseconds;

            return texture;

        }

    }
}
