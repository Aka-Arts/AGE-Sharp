using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using AkaArts.AgeSharp.Utils.Collision;
using Microsoft.Xna.Framework;
using AkaArts.AgeSharp.Utils.Commanding;

namespace Tester
{
    class Program : IBaseCommandable
    {
        private static Boolean isExitRequested = false;
        static void Main(string[] args)
        {
            var prgrm = new Program();
            var cmdCtrl = new CommandController(prgrm);

            do
            {
                Console.WriteLine("enter command or use command 'exit' to stop ...");

                var cmd = Command.Create(Console.ReadLine());
                cmdCtrl.QueueCommand(cmd);

                // is now internal --> cmdCtrl.ProcessQueue();

            } while (!isExitRequested);
            Console.WriteLine("BYE!");
            Console.ReadLine();
        }

        public Program()
        {

        }

        public void RequestExit()
        {
            isExitRequested = true;
        }
    }
}
