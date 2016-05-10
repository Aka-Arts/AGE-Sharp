using AkaArts.AgeSharp.Utils.Commanding;
using AkaArts.AgeSharp.Utils.Console;
using AkaArts.AgeSharp.Utils.Input;
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
        public IInputMapper InputMapper;
        public static AgeConsole Console { get; private set; }
        private ConsoleInputMapper consoleMapper;
        public bool IsConsoleOpen { get; private set; } = false;
        public SpriteBatch SpriteBatch { get; private set; }
        public int WindowHeight { get; private set; } = 600;
        public int WindowWidth { get; private set; } = 1000;
        public GraphicsDeviceManager GraphicsManager { get; private set; }
        public static String TextInputSinceLastFrame { get; private set; } = "";
        private String TextInputBuffer = "";

        public AgeApplication() : base()
        {
            this.CommandController = new CommandController(this);
            this.GraphicsManager = new GraphicsDeviceManager(this);
            this.GraphicsManager.IsFullScreen = false;
            this.GraphicsManager.PreferredBackBufferHeight = this.WindowHeight;
            this.GraphicsManager.PreferredBackBufferWidth = this.WindowWidth;
            this.Window.TextInput += (o, args) => appendToTextBuffer(args);
        }

        private void appendToTextBuffer(TextInputEventArgs args)
        {
            TextInputBuffer += args.Character.ToString();
        }

        protected override void Initialize()
        {
            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            Console = new AgeConsole(this);
            this.consoleMapper = new ConsoleInputMapper(Console, this);

            var defaultContentLoader = new Content.AgeDefaultContent(this.GraphicsDevice);
            defaultContentLoader.InitDefaults();

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
            consoleMapper.Update(gameTime, this.CommandController);

            if (!IsConsoleOpen)
            {
                if (InputMapper != null)
                {
                    InputMapper.Update(gameTime, this.CommandController);
                }
            }
            
            CommandController.ProcessQueue();

            TextInputSinceLastFrame = TextInputBuffer;
            TextInputBuffer = "";
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (IsConsoleOpen)
            {
                Console.Draw(gameTime, SpriteBatch);
            }            

            base.Draw(gameTime);
        }

        public void ToggleConsole()
        {
            IsConsoleOpen = !IsConsoleOpen;
        }

        public void OpenConsole()
        {
            IsConsoleOpen = true;
        }

        public void CloseConsole()
        {
            IsConsoleOpen = false;
        }

        public void RequestExit()
        {
            this.Exit();
        }
    }
}
