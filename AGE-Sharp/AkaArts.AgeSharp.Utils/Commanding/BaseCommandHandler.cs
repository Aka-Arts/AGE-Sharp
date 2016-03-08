using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils.Commanding
{
    class BaseCommandHandler : ICommandHandler
    {
        private Dictionary<String, Action<Command>> commands = new Dictionary<String, Action<Command>>();
        private IBaseCommandable target;

        public BaseCommandHandler(IBaseCommandable target)
        {
            this.target = target;
            commands.Add("exit", CmdExit);
        }

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

        private void CmdExit(Command cmd)
        {
            target.RequestExit();
        }
    }

    public interface IBaseCommandable
    {
        void RequestExit();
    }
}
