using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils.Commanding
{
    class AgeCommandHandler : BaseCommandHandler
    {
        private AgeApplication target;

        public AgeCommandHandler(AgeApplication target)
        {
            this.target = target;
            commands.Add("exit", CmdExit);
            commands.Add("close", CmdCloseConsole);
        }

        private void CmdExit(Command cmd)
        {
            target.RequestExit();
        }

        private void CmdCloseConsole(Command cmd)
        {
            target.CloseConsole();
        }
    }
}
