using AkaArts.AgeSharp.Utils.Collision;
using AkaArts.AgeSharp.Utils.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TesterGraphical.CollisionTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CollisionTest : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PrimitiveDrawer drawer;

        SpriteFont font;

        Polygon2D poly1;
        Polygon2D poly2;

        Vector2 playerPosition; // poly1pos
        Vector2 poly2position;
        Vector2 push = Vector2.Zero;
        double pushDist = 0;

        Vector2 o2o = Vector2.Zero;

        bool playerIntersects = false;

        public CollisionTest()
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

            this.drawer = new PrimitiveDrawer(GraphicsDevice);

            this.Window.Title = "collision test";

            var vertices = new List<Vector2>();
            vertices.Add(new Vector2(30, -30));
            vertices.Add(new Vector2(-30, -30));
            vertices.Add(new Vector2(-30, 30));
            vertices.Add(new Vector2(30, 30));

            this.poly1 = new Polygon2D(GraphicsDevice, new Vector2(50, 50), vertices);
            this.poly2position = new Vector2(500, 300);
            this.poly2 = new Polygon2D(GraphicsDevice, poly2position, vertices);

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
            {
                Exit();
            }

            var mouse = Mouse.GetState();
            var mouseVector = mouse.Position.ToVector2();
            var requiresUpdate = false;

            if (!playerPosition.Equals(mouseVector) ||
                Keyboard.GetState().IsKeyDown(Keys.F4))
            {
                playerPosition = mouseVector;
                requiresUpdate = true;
                o2o = Vector2.Subtract(poly2.Origin, playerPosition);
            }

            if (requiresUpdate)
            {
                testCollisions();
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

            this.drawer.DrawLine(new LineSegment2D(poly2.Origin, playerPosition), Color.Cyan);
            poly2.Draw(Color.LightSlateGray);

            if (playerIntersects)
            {
                poly1.Draw(Color.Red);
            }
            else
            {
                poly1.Draw(Color.Lime);
            }


            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Push Vector: " + push, new Vector2(5, 5), Color.White);
            spriteBatch.DrawString(font, "Push Distance: " + pushDist, new Vector2(5, 25), Color.White);
            //spriteBatch.DrawString(font, "Total visibility tests: " + tests, new Vector2(305, 5), Color.White);
            //spriteBatch.DrawString(font, "Total calculation time: " + calcTime + " ms", new Vector2(505, 5), Color.White);
            //spriteBatch.DrawString(font, "Multithreading (F12) enabled: " + useMultithreading, new Vector2(745, 5), useMultithreading ? Color.Lime : Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void testCollisions()
        {
            poly1.Origin = playerPosition;
            var result = Polygon2D.Intersect(poly1, poly2, o2o);
            playerIntersects = result.Intersects;
            push = result.PushVector;
            pushDist = result.PushDistance;
            if (playerIntersects)
            {
                poly1.Origin = playerPosition + result.PushVector;
            }
        }
    }
}
