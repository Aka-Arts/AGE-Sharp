using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils.Commanding
{
    public static class CommandController
    {
        private static Queue<Command> commandQueue = new Queue<Command>();
        private static List<ICommandHandler> commandHandlers = new List<ICommandHandler>();
        private static Dictionary<String, ICommandHandler> commandMapping = new Dictionary<string, ICommandHandler>();

        /// <summary>
        /// Register internal ICommandHandlers
        /// </summary>
        static CommandController()
        {

        }

        public static void AddCommand(Command cmd)
        {
            commandQueue.Enqueue(cmd);
        }

        public static void ProcessCommands()
        {
            foreach (var command in commandQueue)
            {

            }
        }
    }
}
