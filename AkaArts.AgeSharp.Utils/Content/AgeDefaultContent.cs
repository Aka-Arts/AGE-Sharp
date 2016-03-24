using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AkaArts.AgeSharp.Utils.Content
{
    public class AgeDefaultContent : ContentManager
    {
        public static SpriteFont FONT { get; private set; }

        private class FakeGraphicsService : IGraphicsDeviceService
        {
            internal FakeGraphicsService(GraphicsDevice graphicDevice)
            {
                GraphicsDevice = graphicDevice;
            }

            public GraphicsDevice GraphicsDevice { get; private set; }

#pragma warning disable 67
            public event EventHandler<EventArgs> DeviceCreated;
            public event EventHandler<EventArgs> DeviceDisposing;
            public event EventHandler<EventArgs> DeviceReset;
            public event EventHandler<EventArgs> DeviceResetting;
#pragma warning restore 67
        }

        private class FakeServiceProvider : IServiceProvider
        {
            GraphicsDevice _graphicDevice;
            internal FakeServiceProvider(GraphicsDevice graphicDevice)
            {
                _graphicDevice = graphicDevice;
            }

            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(IGraphicsDeviceService))
                    return new FakeGraphicsService(_graphicDevice);

                throw new NotImplementedException();
            }
        }

        private MemoryStream xnbStream;

        internal AgeDefaultContent(GraphicsDevice graphicDevice)
            : base(new FakeServiceProvider(graphicDevice), "Content")
        {
        }

        internal void InitDefaults()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            var defaultFontStream = currentAssembly.GetManifestResourceStream("AkaArts.AgeSharp.Utils.Content.Monospace-12-regular.xnb");

            FONT = this.LoadFromStream<SpriteFont>(defaultFontStream);
            FONT.DefaultCharacter = '?';
        }

        private T LoadFromStream<T>(Stream stream)
        {
            xnbStream = new MemoryStream();
            stream.CopyTo(xnbStream);
            xnbStream.Seek(0, SeekOrigin.Begin);
            stream.Dispose();

            var result = this.Load<T>("DUMMYSTRING");

            xnbStream.Dispose();
            return result;
        }

        protected override Stream OpenStream(string assetName)
        {
            return xnbStream;
        }
    }
}
