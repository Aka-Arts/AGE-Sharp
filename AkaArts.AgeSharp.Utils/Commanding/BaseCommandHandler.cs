using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkaArts.AgeSharp.Utils.Commanding
{
    public class BaseCommandHandler
    {
        protected Dictionary<String, Action<Command>> commands = new Dictionary<string, Action<Command>>();

        public List<String> GetRegistredCommands()
        {
            return commands.Keys.ToList();
        }

        public void Handle(Command cmd)
        {
            Action<Command> action;
            if (commands.TryGetValue(cmd.Instruction, out action))
            {
                action.Invoke(cmd);
            }
        }
    }
}
