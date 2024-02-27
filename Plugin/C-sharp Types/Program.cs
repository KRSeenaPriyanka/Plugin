using System;

namespace C_sharp_Types
{
    class Program
    {
        static void Main(string[] args)
        {
            string s1 = "10.22";
            float f1 = float.Parse(s1);
            string s2 = "True";
            bool b1 = Convert.ToBoolean(s2);
            char c1 = 'n';
            string s4 = Convert.ToString(c1);
            int age = 24;

            Console.WriteLine(f1); //fetch the output//
            Console.WriteLine(b1);
            Console.WriteLine(s4);
            Console.WriteLine(age);
            Console.ReadLine(); // to read the value - it presents and goes away//

        }
    }
}
