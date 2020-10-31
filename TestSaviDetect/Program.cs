using System;
using System.IO;
using static System.Console;

namespace TestSaviDetect
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sw = new StreamWriter(@"z:\scratch\SaviDetectTest.log"))
            {
                sw.WriteLine("Hello SaviDetect!");
            }
        }
    }
}
