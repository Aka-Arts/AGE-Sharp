using AkaArts.AgeSharp.Utils.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesterGraphical.MapTest
{
    public class SimplexMap
    {
        GraphicsDevice graphics;
        Random rand = new Random();

        float[,] heightMap;
        Texture2D map;
        
        int octaves = 7;
        int dimensions = 512;
        public int Seed { get; private set; }
        int width;
        int height;

        float scale = 0.002f;
        float zoom = 0.0005f;
        float roughness = 0.6f;
        float roughnessSteps = 0.025f;

        float seaLevel = 0.35f;
        float treeline = 0.55f;
        float snowline = 0.65f;
        float levelPerScroll = 0.01f;

        bool needsRecalc = true;
        bool needsRepaint = false;

        public SimplexMap(GraphicsDevice g, int seed, int width, int height)
        {
            graphics = g;
            this.Seed = seed;
            this.width = width;
            this.height = height;
        }

        public Texture2D GetTexture()
        {
            if (needsRecalc)
            {
                needsRecalc = false;
                Recalc();
            }
            else if (needsRepaint)
            {
                needsRepaint = false;
                Repaint();       
            }
            return map;
        }

        public void Reseed(int? newSeed = null)
        {
            if (newSeed.HasValue)
            {
                Seed = newSeed.Value;
            }
            else
            {
                Seed = rand.Next();
            }
            needsRecalc = true;
        }
        
        public void Dispose()
        {
            this.map.Dispose();
        }

        private void Repaint()
        {
            var width = heightMap.GetLength(0);
            var height = heightMap.GetLength(1);

            var texture = new Texture2D(graphics, width, height);

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

            setMapTexture(texture);
        }

        private void Recalc()
        {
            var simplex = new SimplexMapGenerator(Seed);

            var heightMap = simplex.GenerateMap(0, 0, width, height, octaves, roughness, scale);

            var texture = new Texture2D(graphics, width, height);

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

            setMapTexture(texture);
        }

        private void setMapTexture(Texture2D t)
        {
            if (this.map != null)
            {
                this.map.Dispose();
            }
            this.map = t;
        }
    }
}
