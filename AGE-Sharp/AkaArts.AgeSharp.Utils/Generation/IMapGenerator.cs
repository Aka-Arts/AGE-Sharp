using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils.Generation
{
    interface IMapGenerator
    {

        float[][] Generate(int width, int height);

    }
}
