using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;

namespace LittleOrange.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            IApplicationContext ctx = ContextRegistry.GetContext();

            Console.Out.WriteLine("Server listening...");

            Console.ReadLine();
        }
    }
}
