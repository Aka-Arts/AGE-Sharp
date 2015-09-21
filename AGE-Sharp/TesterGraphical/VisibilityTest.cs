using AkaArts.AgeSharp.Utils.Collision;
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
    public class VisibilityTest : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PrimitiveDrawer drawer;

        SpriteFont font;

        List<LineSegment2D> lines = new List<LineSegment2D>();
        List<LineSegment2D> rays = new List<LineSegment2D>();

        List<Vector2> allVertices = new List<Vector2>();

        Vector2 playerPosition;

        int tests = 0, visibles = 0;

        long calcTime = 0;

        bool readyForRecalc = true;

        Random random = new Random();

        Stopwatch stopwatch = new Stopwatch();

        public VisibilityTest()
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
            playerPosition = new Vector2(random.Next(0, 1001), random.Next(0, 601));
            calcLines(true);

            this.drawer = new PrimitiveDrawer(GraphicsDevice);

            this.Window.Title = "Visibility tests benchmark";

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

                calcLines(true);

            }

            if (keys.IsKeyUp(Keys.F4))
            {
                readyForRecalc = true;    
            }

            var mouse = Mouse.GetState();

            var mouseVector = mouse.Position.ToVector2();

            var requiresUpdate = false;

            if (!playerPosition.Equals(mouseVector))
            {
                playerPosition = mouseVector;
                requiresUpdate = true;
            }

            if (requiresUpdate)
            {
                calcLines();
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

            foreach (var line in lines)
            {
                drawer.DrawLine(line, Color.DarkGray);
            }

            foreach (var line in rays)
            {
                drawer.DrawLine(line, Color.Red);
            }

            spriteBatch.Begin();

                spriteBatch.DrawString(font, "Total vertices: " + allVertices.Count, new Vector2(5, 5), Color.White);
                spriteBatch.DrawString(font, "Total visible: " + visibles, new Vector2(205, 5), Color.White);
                spriteBatch.DrawString(font, "Total visibility tests: " + tests, new Vector2(405, 5), Color.White);
                spriteBatch.DrawString(font, "Total calculation time: " + calcTime + " milliseconds", new Vector2(605, 5), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void calcLines(bool newPolygons = false)
        {

            rays = new List<LineSegment2D>();

            stopwatch.Restart();

            if (newPolygons)
            {

                lines = new List<LineSegment2D>();

                allVertices = new List<Vector2>();

                // 64 objects are perfect, trust me!
                int numberOfPolygons = 64;

                for (int i = 0; i < numberOfPolygons; i++)
                {

                    var originX = random.Next(20, 981);
                    var originY = random.Next(40, 581);

                    var verticesCount = random.Next(3, 8);

                    Vector2[] vertices = new Vector2[verticesCount];

                    for (int j = 0; j < verticesCount; j++)
                    {
                        var x = random.Next(-20, 21);
                        var y = random.Next(-20, 21);
                        vertices[j] = new Vector2(originX + x, originY + y);
                    }

                    for (int j = 0; j < vertices.Length; j++)
                    {
                        Vector2 vec1, vec2;

                        vec1 = vertices[j];

                        if (j + 1 >= vertices.Length)
                        {
                            vec2 = vertices[0];
                        }
                        else
                        {
                            vec2 = vertices[j + 1];
                        }

                        lines.Add(new LineSegment2D(vec1, vec2));
                        allVertices.Add(vec1);
                    }


                }
   
            }

            tests = 0;
            visibles = 0;

            for (int i = 0; i < allVertices.Count - 1; i++)
            {

                var line = new LineSegment2D(playerPosition, allVertices[i]);

                var visible = true;

                for (int j = 0; j < lines.Count; j++)
                {
                    tests++;

                    if (line.Intersects(lines[j]).intersects)
                    {
                        visible = false;
                        break;
                    }

                }

                if (visible)
                {
                    rays.Add(line);
                    visibles++;
                }

            }

            stopwatch.Stop();

            calcTime = stopwatch.ElapsedMilliseconds;

        }

    }
}
