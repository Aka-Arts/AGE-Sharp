using AkaArts.AgeSharp.Utils.Commanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesterGraphical
{
    class MapCommandHandler : ICommandHandler
    {
        private Dictionary<String, Action<Command>> commands = new Dictionary<string, Action<Command>>();
        private SimplexMap targetMap;

        public MapCommandHandler(SimplexMap targetMap)
        {
            this.targetMap = targetMap;
            commands.Add("map", cmdMap);
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

        private void cmdMap(Command command)
        {
            if (command.Arguments.Count > 0)
            {
                var firstArg = command.Arguments[0].ToLowerInvariant();
                switch (firstArg)
                {
                    case "reseed":

                        int? givenSeed = null;
                        int parsed = 0;

                        if (command.Arguments.Count > 1 &&
                            int.TryParse(command.Arguments[1], out parsed))
                        {
                            givenSeed = parsed;
                        }

                        targetMap.Reseed(givenSeed);

                        MapTest.Console.WriteLine("Map was reseeded with new seed of " + targetMap.Seed);
                        break;
                    default:
                        MapTest.Console.WriteLine("Unknown argument for command 'map': " + firstArg);
                        break;
                }
            }
        }
    }
}
