using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils.Commanding
{
    class BaseCommandHandler : ICommandHandler
    {
        public List<String> GetRegistredCommands()
        {
            throw new NotImplementedException();
        }

        public void Handle(String cmd)
        {
            throw new NotImplementedException();
        }
    }
}
