using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkaArts.AgeSharp.Utils.Input
{
    public interface IInputMapper
    {
        void Update(GameTime gameTime, Commanding.CommandController target);
    }
}
