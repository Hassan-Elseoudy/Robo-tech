using System;
using System.Collections.Generic;
using System.Text;

using IronPython.Hosting;

namespace RoboTech_Connection
{

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Press enter to execute the python script!");
            Console.ReadLine();

            try
            {
                var engine = Python.CreateEngine();
                var scope = engine.CreateScope();
                engine.ExecuteFile (@"script.py", scope);

                // get function and dynamically invoke
                var calcAdd = scope.GetVariable("CalcAdd");
                var result = calcAdd(34, 8); // returns 42 (Int32)

                // get function with a strongly typed signature
                var calcAddTyped = scope.GetVariable<Func<decimal, decimal, decimal>>("CalcAdd");
                var resultTyped = calcAddTyped(5, 7);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                   "Oops! We couldn't execute the script because of an exception: " + ex.Message);
            }

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}