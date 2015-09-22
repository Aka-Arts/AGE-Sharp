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

        float[,] heightMap;
        Texture2D map;

        Stopwatch stopwatch = new Stopwatch();

        int windowHeight = 600;
        int windowWidth = 1000;

        int octaves = 7;
        int dimensions = 512;
        int currentSeed = 1;

        float scale = 0.002f;
        float zoom = 0.0005f;
        float roughness = 0.6f;
        float roughnessSteps = 0.025f;

        float seaLevel = 0.35f;
        float treeline = 0.65f;
        float snowline = 0.8f;
        float levelPerScroll = 0.01f;

        int wheelValue = 0;

        KeyboardState previousFrame;

        Keys reseed = Keys.F4;
        Keys addOct = Keys.PageUp;
        Keys remOct = Keys.PageDown;
        Keys addRough = Keys.Up;
        Keys remRough = Keys.Down;
        Keys scrollSeaLevel = Keys.A;
        Keys scrollTreeline = Keys.S;
        Keys scrollSnowline = Keys.D;

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

            heightMap = calcMap(currentSeed, dimensions, dimensions, octaves, roughness, scale);
            map = repaintMap(heightMap);

            previousFrame = Keyboard.GetState();
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
            bool doRepaint = false;

            if (IsActive)
            {
                var keys = Keyboard.GetState();

                var mouse = Mouse.GetState();

                // reseed
                if (keys.IsKeyDown(reseed) && !previousFrame.IsKeyDown(reseed))
                {
                    currentSeed = rand.Next();

                    doRecalc = true;
                }

                // octaves
                if (keys.IsKeyDown(addOct) && !previousFrame.IsKeyDown(addOct))
                {
                    doRecalc = true;

                    octaves++;
                }

                if (keys.IsKeyDown(remOct) && !previousFrame.IsKeyDown(remOct))
                {
                    octaves--;

                    if (octaves < 1)
                    {
                        octaves = 1;
                    }

                    doRecalc = true;
                }

                // roughness
                if (keys.IsKeyDown(addRough) && !previousFrame.IsKeyDown(addRough))
                {
                    doRecalc = true;

                    roughness += roughnessSteps;

                    if (roughness > 1)
                    {
                        roughness = 1;
                    }
                }

                if (keys.IsKeyDown(remRough) && !previousFrame.IsKeyDown(remRough))
                {
                    roughness -= roughnessSteps;

                    if (roughness < 0)
                    {
                        roughness = 0;
                    }

                    doRecalc = true;
                }

                // wheel
                if (mouse.ScrollWheelValue != wheelValue)
                {

                    if (keys.IsKeyDown(scrollSeaLevel))
                    {
                        // sealevel
                        if (mouse.ScrollWheelValue > wheelValue)
                        {
                            seaLevel += levelPerScroll;
                            if (seaLevel > treeline - levelPerScroll)
                            {
                                seaLevel = treeline - levelPerScroll;
                            }
                        }
                        else
                        {
                            seaLevel -= levelPerScroll;
                            if (seaLevel < 0)
                            {
                                seaLevel = 0;
                            }
                        }

                        doRepaint = true;
                    }
                    else if (keys.IsKeyDown(scrollTreeline))
                    {
                        // treeline
                        if (mouse.ScrollWheelValue > wheelValue)
                        {
                            treeline += levelPerScroll;
                            if (treeline > snowline - levelPerScroll)
                            {
                                treeline = snowline - levelPerScroll;
                            }
                        }
                        else
                        {
                            treeline -= levelPerScroll;
                            if (treeline < seaLevel + levelPerScroll)
                            {
                                treeline = seaLevel + levelPerScroll;
                            }
                        }

                        doRepaint = true;
                    }
                    else if (keys.IsKeyDown(scrollSnowline))
                    {
                        // snowline
                        if (mouse.ScrollWheelValue > wheelValue)
                        {
                            snowline += levelPerScroll;
                            if (snowline > 1)
                            {
                                snowline = 1;
                            }
                        }
                        else
                        {
                            snowline -= levelPerScroll;
                            if (snowline < treeline + levelPerScroll)
                            {
                                snowline = treeline + levelPerScroll;
                            }
                        }

                        doRepaint = true;
                    }
                    else
                    {
                        // zoom
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

                        doRecalc = true;
                    }

                    wheelValue = mouse.ScrollWheelValue;
                }

                previousFrame = keys;
            }

            if (doRecalc)
            {
                this.heightMap = calcMap(currentSeed, dimensions, dimensions, octaves, roughness, scale);
                this.map = repaintMap(this.heightMap);
            }

            if (doRepaint)
            {
                this.map = repaintMap(this.heightMap);
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
            spriteBatch.DrawString(font, "Seed: " + currentSeed, new Vector2(5, 45), Color.White);
            spriteBatch.DrawString(font, "Sea Level: " + seaLevel, new Vector2(5, 45), Color.White);
            spriteBatch.DrawString(font, "Treeline: " + treeline, new Vector2(5, 65), Color.White);
            spriteBatch.DrawString(font, "Snowline: " + snowline, new Vector2(5, 85), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private float[,] calcMap(int seed, int width, int height, int octaves, float roughness, float scale)
        {

            stopwatch.Restart();

            var simplex = new SimplexMapGenerator(seed);

            var heightMap = simplex.Generate(width, height, octaves, roughness, scale);

            var texture = new Texture2D(GraphicsDevice, width, height);

            var pixels = new Color[width * height];

            for (int i = 0 ; i < width ; i++)
            {
                for (int j = 0 ; j < height ; j++)
                {

                    pixels[(i * width) + j] = Color.Navy;

                    if (heightMap[i,j] > seaLevel)
                    {

                        pixels[(i * width) + j] = Color.DarkGreen;

                    }

                    if (heightMap[i, j] > treeline)
                    {

                        pixels[(i * width) + j] = Color.Gray;

                    }

                    if (heightMap[i, j] > snowline)
                    {

                        pixels[(i * width) + j] = Color.White;

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

            return heightMap;

        }

        private Texture2D repaintMap(float[,] heightMap)
        {

            var width = heightMap.GetLength(0);
            var height = heightMap.GetLength(1);

            var texture = new Texture2D(GraphicsDevice, width, height);

            var pixels = new Color[width * height];

            for (int i = 0 ; i < width ; i++)
            {
                for (int j = 0 ; j < height ; j++)
                {

                    pixels[(i * width) + j] = Color.Navy;

                    if (heightMap[i, j] > seaLevel)
                    {

                        pixels[(i * width) + j] = Color.DarkGreen;

                    }

                    if (heightMap[i, j] > treeline)
                    {

                        pixels[(i * width) + j] = Color.Gray;

                    }

                    if (heightMap[i, j] > snowline)
                    {

                        pixels[(i * width) + j] = Color.White;

                    }

                    //if (heightMap[(i + 1) % width,(j + 1) % height] > heightMap[i, j])
                    //{
                    //    pixels[(i * width) + j] = Color.Lerp(pixels[(i * width) + j], Color.Black, 0.5f);
                    //}

                }
            }

            texture.SetData<Color>(pixels);

            return texture;

        }

    }
}
