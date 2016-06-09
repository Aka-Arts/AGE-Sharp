using System;
using System.Collections.Generic;
using System.Linq;

namespace TesterGraphical
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new VisibilityTest.VisibilityTest())
                game.Run();
        }
    }
#endif
}
