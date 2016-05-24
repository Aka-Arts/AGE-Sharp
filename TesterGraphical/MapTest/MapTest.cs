using AkaArts.AgeSharp.Utils.Graphics;
using AkaArts.AgeSharp.Utils.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using AkaArts.AgeSharp.Utils;
using AkaArts.AgeSharp.Utils.Commanding;

namespace TesterGraphical.MapTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MapTest : AgeApplication
    {
        SpriteFont font;

        long calcTime = 0;

        Random rand = new Random();

        SimplexMap map;

        Stopwatch stopwatch = new Stopwatch();

        int octaves = 7;
        int dimensions = 512;
        int currentSeed = 1;

        float scale = 0.002f;
        float zoom = 0.0005f;
        float roughness = 0.6f;
        float roughnessSteps = 0.025f;

        float seaLevel = 0.35f;
        float treeline = 0.55f;
        float snowline = 0.65f;
        float levelPerScroll = 0.01f;

        public MapTest()
            : base()
        {
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void OnAgeInitialized()
        {
            this.Window.Title = "perlin map tests benchmark";

            map = new SimplexMap(GraphicsDevice, currentSeed, dimensions, dimensions);
            CommandController.AddCommandHandler(new MapCommandHandler(map));

            this.InputHandler = new DemoInputHandler(CommandController);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            font = AkaArts.AgeSharp.Utils.Content.AgeDefaultContent.FONT; //Content.Load<SpriteFont>("fonts/Arial-12-regular");


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            map.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void OnAgeUpdated(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                CommandController.QueueCommand("exit");            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void OnAgeDraw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();

            SpriteBatch.Draw(map.GetTexture(), new Vector2((WindowWidth / 2) - dimensions / 2, (WindowHeight / 2) - dimensions / 2), Color.White);

            SpriteBatch.DrawString(font, "Total calculation time: " + calcTime + " milliseconds", new Vector2(5, 5), Color.White);
            SpriteBatch.DrawString(font, "Simplex: " + octaves + " octaves", new Vector2(450, 5), Color.White);
            SpriteBatch.DrawString(font, "Roughness: " + roughness, new Vector2(5, 25), Color.White);
            SpriteBatch.DrawString(font, "Zoom: " + scale, new Vector2(450, 25), Color.White);
            SpriteBatch.DrawString(font, "Seed: " + map.Seed, new Vector2(5, 45), Color.White);
            SpriteBatch.DrawString(font, "Sea Level: " + seaLevel, new Vector2(5, 65), Color.White);
            SpriteBatch.DrawString(font, "Treeline: " + treeline, new Vector2(5, 85), Color.White);
            SpriteBatch.DrawString(font, "Snowline: " + snowline, new Vector2(5, 105), Color.White);

            SpriteBatch.End();
        }

        private float[,] calcMap(int seed, int width, int height, int octaves, float roughness, float scale)
        {

            stopwatch.Restart();

            var simplex = new SimplexMapGenerator(seed);

            var heightMap = simplex.GenerateMap(0, 0, width, height, octaves, roughness, scale);

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
