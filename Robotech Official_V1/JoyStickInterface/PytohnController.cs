using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using IronPython.Hosting;

namespace JoyStickInterface
{

    public class PythonController
    {
        public static void controlShapes()
        {
            var engine = Python.CreateEngine();
            var searchPaths = engine.GetSearchPaths();
            var scope = engine.CreateScope();
            searchPaths.Add(@"C:\Users\smsm\AppData\Local\Programs\Python\Python37-32\Lib\site-packages");

            engine.ExecuteFile(@"detect_count.py", scope);
            // get function and dynamically invoke
            var calcAdd = scope.GetVariable("main");
            var result = calcAdd();
            MessageBox.Show(Convert.ToString(result));

        }
        public static void control()
        {
            var engine = Python.CreateEngine();
            var scope = engine.CreateScope();
            engine.ExecuteFile(@"script.py", scope);

            // get function and dynamically invoke
            var calcAdd = scope.GetVariable("CalcAdd");
            var result = calcAdd(34, 8); // returns 42 (Int32)
            MessageBox.Show(Convert.ToString(result));
        }
    }
}