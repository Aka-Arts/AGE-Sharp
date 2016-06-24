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

        List<Polygon2D> worldObjects = new List<Polygon2D>();

        Polygon2D player;
        Polygon2D playerRequested;
        Polygon2D playerEffective;
        Polygon2D hull;

        Vector2 playerPosition; // poly1pos
        Vector2 playerRequestedPosition;

        Vector2 push = Vector2.Zero;
        double pushDist = 0;
        double distTmp = 0;

        Vector2 player2poly2 = Vector2.Zero;
        Vector2 player2poly3 = Vector2.Zero;

        bool playerIntersects = false;

        KeyboardState previousKeys;
        MouseState previousMouse;

        public CollisionTest()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 300;

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

            previousKeys = Keyboard.GetState();
            previousMouse = Mouse.GetState();

            playerPosition = new Vector2(251, 51);

            var vertices = new List<Vector2>();
            vertices.Add(new Vector2(-30, -30));
            vertices.Add(new Vector2(-30, 30));
            vertices.Add(new Vector2(30, 30));
            vertices.Add(new Vector2(30, -30));
            //vertices.Add(new Vector2(0, -30));

            this.player = new Polygon2D(GraphicsDevice, playerPosition, vertices);
            this.playerRequested = new Polygon2D(GraphicsDevice, playerPosition, vertices);
            this.playerEffective = new Polygon2D(GraphicsDevice, playerPosition, vertices);
            var poly1position = new Vector2(200, 150);
            var poly1 = new Polygon2D(GraphicsDevice, poly1position, vertices);
            var poly2position = new Vector2(300, 250);
            var poly2 = new Polygon2D(GraphicsDevice, poly2position, vertices);

            worldObjects.Add(poly1);
            worldObjects.Add(poly2);

            var playerPoints = new List<Vector2>(player.AbsoluteVertices);
            playerPoints.AddRange(playerRequested.AbsoluteVertices);

            this.hull = new Polygon2D(GraphicsDevice, Vector2.Zero, Polygon2D.GetConvexHull(playerPoints));

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
            var currentKeys = Keyboard.GetState();
            var currentMouse = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentKeys.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (this.IsActive)
            {
                var recalcHull = false;

                if (previousKeys.IsKeyDown(Keys.Up) &&
                    currentKeys.IsKeyUp(Keys.Up))
                {
                    playerPosition = playerPosition + new Vector2(0, -10);
                    recalcHull = true;
                }

                if (previousKeys.IsKeyDown(Keys.Down) &&
                    currentKeys.IsKeyUp(Keys.Down))
                {
                    playerPosition = playerPosition + new Vector2(0, 10);
                    recalcHull = true;
                }

                if (previousKeys.IsKeyDown(Keys.Left) &&
                    currentKeys.IsKeyUp(Keys.Left))
                {
                    playerPosition = playerPosition + new Vector2(-10, 0);
                    recalcHull = true;
                }

                if (previousKeys.IsKeyDown(Keys.Right) &&
                    currentKeys.IsKeyUp(Keys.Right))
                {
                    playerPosition = playerPosition + new Vector2(10, 0);
                    recalcHull = true;
                }
                
                var mouseVector = currentMouse.Position.ToVector2();

                if (currentMouse.LeftButton == ButtonState.Pressed) //mouseVector != playerRequestedPosition)
                {
                    playerRequestedPosition = mouseVector;
                    playerRequested.Origin = playerRequestedPosition;
                    recalcHull = true;
                }
                else if (previousMouse.LeftButton == ButtonState.Pressed)
                {
                    playerPosition = playerEffective.Origin;
                    player.Origin = playerPosition;
                    recalcHull = true;
                }

                if (recalcHull || previousKeys.IsKeyDown(Keys.F4) && currentKeys.IsKeyUp(Keys.F4))
                {
                    player.Origin = playerPosition;
                    calcPlayerHull();
                    testCollisions();
                }

                previousKeys = currentKeys;
                previousMouse = currentMouse;
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

            this.drawer.DrawLine(new LineSegment2D(playerPosition, playerRequestedPosition), Color.Cyan);

            worldObjects.ForEach((x) => x.Draw(Color.SlateGray));

            player.Draw(Color.Aquamarine);
            playerRequested.Draw(Color.Aqua);
            playerEffective.Draw(Color.DarkGoldenrod);

            if (playerIntersects)
            {
                hull.Draw(Color.Red);
            }
            else
            {
                hull.Draw(Color.Lime);
            }


            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Push Vector: " + push, new Vector2(5, 5), Color.White);
            spriteBatch.DrawString(font, "Push Distance: " + pushDist, new Vector2(5, 25), Color.White);
            spriteBatch.DrawString(font, "Player to Poly vector: " + player2poly2, new Vector2(5, 45), Color.White);
            spriteBatch.DrawString(font, "distTmp: " + distTmp, new Vector2(5, 65), Color.White);
            //spriteBatch.DrawString(font, "Total visibility tests: " + tests, new Vector2(305, 5), Color.White);
            //spriteBatch.DrawString(font, "Total calculation time: " + calcTime + " ms", new Vector2(505, 5), Color.White);
            //spriteBatch.DrawString(font, "Multithreading (F12) enabled: " + useMultithreading, new Vector2(745, 5), useMultithreading ? Color.Lime : Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void testCollisions()
        {
            playerIntersects = false;

            var direction = playerRequestedPosition - playerPosition;
            var biggestPush = 0d;
            var biggestPushVector = Vector2.Zero;

            worldObjects.ForEach((x) =>
            {
                var result = Polygon2D.Intersect(x, hull, direction);
                playerIntersects = result.Intersects || playerIntersects;

                if (result.PushDistance > biggestPush)
                {
                    biggestPush = result.PushDistance;
                    biggestPushVector = result.PushVector;
                    distTmp = result.distTmp;
                }

            });

            push = biggestPushVector;
            pushDist = biggestPush;

            playerEffective.Origin = playerRequestedPosition + biggestPushVector;
        }

        void calcPlayerHull()
        {
            this.hull = player + playerRequested;
        }
    }
}
