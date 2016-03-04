using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils.Commanding
{
    public interface ICommandHandler
    {
        void Handle(String cmd);
        List<String> GetRegistredCommands();
    }
}
