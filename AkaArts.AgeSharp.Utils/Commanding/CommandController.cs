using AkaArts.AgeSharp.Utils.Console;
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

        public void QueueCommand(String cmdString)
        {
            QueueCommand(Command.Create(cmdString));
        }

        public void QueueCommand(Command cmd)
        {
            if (cmd != null)
            {
                commandQueue.Enqueue(cmd);
            }
        }

        internal void ProcessQueue()
        {
            if (commandQueue.Count < 1)
            {
                return;
            }

            var currentQueue = new Queue<Command>(commandQueue);
            commandQueue.Clear();
            foreach (var command in currentQueue)
            {
                ICommandHandler handler;
                if (commandMapping.TryGetValue(command.Instruction, out handler))
                {
                    handler.Handle(command);
                }
                else
                {
                    AgeApplication.Console.WriteLine("Unknown command: " + command.Raw);
                }
            }
        }

        public void AddCommandHandler(ICommandHandler cmdHandler)
        {
            if (commandHandlers.Any(x => x.GetType() == cmdHandler.GetType()))
            {
                throw new CommandingException("There is already a commandHandler of this type registred");
            }

            var commands = cmdHandler.GetRegistredCommands();

            foreach (var cmd in commands)
            {
                if (commandMapping.Any(x => x.Key.Equals(cmd, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new CommandingException("Another commandHandler has already reserved the command '" + cmd + "'");
                }
            }
            // if all good, add all of them to the mapping
            foreach (var cmd in commands)
            {
                commandMapping.Add(cmd.ToLowerInvariant(), cmdHandler);
            }

            commandHandlers.Add(cmdHandler);
        }

        public class CommandingException : Exception
        {
            public CommandingException(String msg) : base(msg) { }
        }
    }
}
