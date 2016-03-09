using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils.Commanding
{
    public class CommandController
    {
        private Queue<Command> commandQueue = new Queue<Command>();
        private List<ICommandHandler> commandHandlers = new List<ICommandHandler>();
        private Dictionary<String, ICommandHandler> commandMapping = new Dictionary<string, ICommandHandler>();

        /// <summary>
        /// Register internal ICommandHandlers
        /// </summary>
        public CommandController(IBaseCommandable app)
        {
            AddCommandHandler(new BaseCommandHandler(app));
        }

        public void QueueCommand(Command cmd)
        {
            commandQueue.Enqueue(cmd);
        }

        public void ProcessQueue()
        {
            foreach (var command in commandQueue)
            {
                ICommandHandler handler;
                if (commandMapping.TryGetValue(command.Instruction, out handler))
                {
                    handler.Handle(command);
                }
                else
                {
                    System.Console.WriteLine("Unknown command: " + command.Instruction);
                }
            }
            commandQueue.Clear();
        }

        public Boolean AddCommandHandler(ICommandHandler cmdHandler)
        {
            if (commandHandlers.Any(x => x.GetType() == cmdHandler.GetType()))
            {
                return false;
            }

            var commands = cmdHandler.GetRegistredCommands();

            foreach (var cmd in commands)
            {
                if (commandMapping.Any(x => x.Key.Equals(cmd, StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
            }
            // if all good, add all of them to the mapping
            foreach (var cmd in commands)
            {
                commandMapping.Add(cmd.ToLowerInvariant(), cmdHandler);
            }

            commandHandlers.Add(cmdHandler);
            return true;
        }
    }
}
