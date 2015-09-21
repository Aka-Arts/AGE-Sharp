using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils.Generation
{
    public class PerlinMapGenerator
    {

        public readonly int seed;

        public PerlinMapGenerator(int seed)
        {

            this.seed = seed;

        }

        public float[,] Generate(int width, int height, int octaves)
        {

            float[,] map = new float[width, height];

            var baseMap = GenerateBaseMap(width, height);

            float[][,] mapByOctaves = new float[octaves][,];

            for (int i = 0 ; i < octaves ; i++)
            {
                mapByOctaves[i] = GenerateOctave(baseMap, i);
            }

            float amplitude = 1f;
            float totalAmplitude = 0f;

            for (int octave = octaves - 1 ; octave >= 0 ; octave--)
            {

                amplitude *= 0.5f;
                totalAmplitude += amplitude;

                for (int i = 0 ; i < width ; i++)
                {

                    for (int j = 0 ; j < height ; j++)
                    {

                        map[i, j] += mapByOctaves[octave][i, j] * amplitude;

                    }

                }

            }

            for (int i = 0 ; i < width ; i++)
            {

                for (int j = 0 ; j < height ; j++)
                {

                    map[i, j] /= totalAmplitude;

                }

            }

            return map;

        }

        private float[,] GenerateBaseMap(int width, int height)
        {

            Random rand = new Random(this.seed);
            float[,] baseMap = new float[width, height];

            for (int i = 0 ; i < width ; i++)
            {

                for (int j = 0 ; j < height ; j++)
                {

                    baseMap[i, j] = (float)rand.NextDouble() % 1;

                }

            }

            return baseMap;

        }

        private float[,] GenerateOctave(float[,] baseMap, int octave)
        {

            int width = baseMap.GetLength(0);
            int height = baseMap.GetLength(1);

            float[,] octaveMap = new float[width,height];

            int period = 1 << octave;
            float frequency = 1f / period;

            for (int i = 0 ; i < width ; i++)
            {

                int xMin = (i / period) * period;
                int xMax = (xMin + period) % width;

                float hBlend = (i - xMin) * frequency;

                for (int j = 0 ; j < height ; j++)
                {

                    int yMin = (j / period) * period;
                    int yMax = (yMin + period) % height;

                    float vBlend = (j - yMin) * frequency;

                    float top = Interpolation.Linear(baseMap[xMin, yMin], baseMap[xMax, yMin], hBlend);

                    float bottom = Interpolation.Linear(baseMap[xMin, yMax], baseMap[xMax, yMax], hBlend);

                    octaveMap[i, j] = Interpolation.Linear(top, bottom, vBlend);

                }

            }

            return octaveMap;

        }

    }
}
