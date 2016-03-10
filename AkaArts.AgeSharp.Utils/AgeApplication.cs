using AkaArts.AgeSharp.Utils.Commanding;
using AkaArts.AgeSharp.Utils.Console;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkaArts.AgeSharp.Utils
{
    public class AgeApplication : Game, IBaseCommandable
    {
        public readonly CommandController CommandController;
        public AgeConsole Console { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public SpriteFont DefaultFont { get; private set; }

        public int WindowHeight { get; private set; } = 600;
        public int WindowWidth { get; private set; } = 1000;
        public GraphicsDeviceManager GraphicsManager { get; private set; }

        public AgeApplication() : base()
        {
            this.CommandController = new CommandController(this);
            this.GraphicsManager = new GraphicsDeviceManager(this);
            this.GraphicsManager.IsFullScreen = false;
            this.GraphicsManager.PreferredBackBufferHeight = this.WindowHeight;
            this.GraphicsManager.PreferredBackBufferWidth = this.WindowWidth;
        }

        protected override void Initialize()
        {
            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Console = new AgeConsole(this);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        {            
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {                        
            this.Console.Draw(gameTime, SpriteBatch);

            base.Draw(gameTime);
        }

        public void RequestExit()
        {
            this.Exit();
        }
    }
}
