using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils.Generation
{
    public static class Interpolation
    {

        public static float Linear(float a, float b, float lerp)
        {

            return a * (1 - lerp) + lerp * b;

        }

    }
}
