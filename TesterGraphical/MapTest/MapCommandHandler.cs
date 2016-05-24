using AkaArts.AgeSharp.Utils.Commanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesterGraphical.MapTest
{
    class MapCommandHandler : BaseCommandHandler
    {        
        private SimplexMap targetMap;

        public MapCommandHandler(SimplexMap targetMap)
        {
            this.targetMap = targetMap;
            commands.Add("map", cmdMap);
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
