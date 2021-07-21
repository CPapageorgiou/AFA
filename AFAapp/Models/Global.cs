using System;
using System.Collections.Generic;

namespace AFAapp.Models
{

    public static class Global
    {
        public static AFA a1 = new AFA(
            "q0",

             new Dictionary<(string, char), string>()

            { { ("q0",'a'), "q1" },

            { ("q0",'b'), "q0 and not q1"},

            { ("q1",'a'), "q0" },

            { ("q1",'b'), "not q0" } },

            new List<string> { "q1" });



        public static string[] booleans = { "0", "1", "true", "false", "True", "TRUE", "False", "FALSE" };

        public static string[] connectives = { "and", "not", "or" };


        public static string[] stringToArray(string s)
        {
            string s1 = s.Replace("(", "( ");
            string s2 = s1.Replace(")", " )");
            string[] subs = s2.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            return subs;
        }


        public static void Print<T>(IEnumerable<T> col)
        {
            foreach (var item in col)
                Console.WriteLine(item);
        }

    }
}

