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
    public class AgeApplication : Game
    {
        public readonly CommandController CommandController;
        public BaseInputHandler InputHandler;
        public static AgeConsole Console { get; private set; }
        private ConsoleInputHandler consoleInputHandler;
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

        #region Overrides

        protected sealed override void Initialize()
        {
            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            Console = new AgeConsole(this);
            this.consoleInputHandler = new ConsoleInputHandler(Console, this);

            var defaultContentLoader = new Content.AgeDefaultContent(this.GraphicsDevice);
            defaultContentLoader.InitDefaults();

            OnAgeInitialized();

            base.Initialize();
        }

        protected virtual void OnAgeInitialized()
        {

        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        {
        }

        protected sealed override void Update(GameTime gameTime)
        {
            consoleInputHandler.Update(gameTime);

            if (!IsConsoleOpen)
            {
                if (InputHandler != null)
                {
                    InputHandler.Update(gameTime);
                }
            }
            
            CommandController.ProcessQueue();

            TextInputSinceLastFrame = TextInputBuffer;
            TextInputBuffer = "";

            OnAgeUpdated(gameTime);

            base.Update(gameTime);
        }

        protected virtual void OnAgeUpdated(GameTime gameTime)
        {

        }

        protected sealed override void Draw(GameTime gameTime)
        {
            OnAgeDraw(gameTime);

            if (IsConsoleOpen)
            {
                Console.Draw(gameTime);
            }            

            base.Draw(gameTime);
        }

        protected virtual void OnAgeDraw(GameTime gameTime)
        {

        }

        #endregion

        #region Publics

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

        #endregion

        #region Privates

        private void appendToTextBuffer(TextInputEventArgs args)
        {
            TextInputBuffer += args.Character.ToString();
        }

        #endregion
    }
}
