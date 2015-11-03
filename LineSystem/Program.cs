using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineSystemCore;
using LineSystemUI;

namespace MainProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            var lineSystem = new LineSystem();
            var UI = new LineSystemCLI(lineSystem);

            lineSystem.Users.Add(new User("Test", "Test", "Test", "Test@Test.Test"));

            UI.Start();
        }
    }
}
