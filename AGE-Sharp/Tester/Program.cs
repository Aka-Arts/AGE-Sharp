using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using AkaArts.AgeSharp.Utils.Collision;
using Microsoft.Xna.Framework;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {

            do
            {
                run();
                Console.WriteLine("press ENTER to rerun or use command 'exit' to stop ...");
            } while (!Console.ReadLine().Equals("exit", StringComparison.InvariantCultureIgnoreCase));

        }

        private static void run()
        {

            var random = new Random();

            var lines = new List<LineSegment2D>();
            var allVertices = new List<Vector2>();

            for (int i = 0; i < 64; i++)
            {

                var originX = random.Next(-100, 101);
                var originY = random.Next(-100, 101);

                var verticesCount = random.Next(3, 8);

                Vector2[] vertices = new Vector2[verticesCount];

                for (int j = 0; j < verticesCount; j++)
                {
                    var x = random.Next(-10, 11);
                    var y = random.Next(-10, 11);
                    vertices[j] = new Vector2(x, y);
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

            var playerPosition = new Vector2(random.Next(-100, 101), random.Next(-100, 101));

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var ops = 0;
            var tests = 0;
            var visibles = 0;

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
                    visibles++;
                }

                ops++;
            }

            stopwatch.Stop();

            Console.WriteLine("Completed in " + stopwatch.ElapsedMilliseconds + " milliseconds with " + ops + " operations, " + allVertices.Count + " total vertices, " + tests + " tests and " + visibles + " visible vertices");

        }
    }
}
