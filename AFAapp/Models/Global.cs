using System;
using System.Collections.Generic;
using System.Linq;

namespace AFAapp.Models
{

    public static class Global
    {

        public static string[] booleans = { "0", "1", "true", "false", "True", "TRUE", "False", "FALSE" };

        public static string[] connectives = { "and", "not", "or" };

        public static string[] connectivesExceptNot = { "and", "or" };


        public static string[] stringToArray(string s)
        {
            string s1 = s.Replace("(", "( ");
            string s2 = s1.Replace(")", " )");
            string[] subs = s2.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            return subs;
        }


        public static List<string> stringToArrayNoPar(string s)
        {
            string s1 = s.Replace("(", "( ");
            string s2 = s1.Replace(")", " )");
            string[] subs = s2.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var strArrNoPar = subs.Where(x => x != "(" && x != ")");
            return strArrNoPar.ToList();
        }


        public static void Print<T>(IEnumerable<T> col)
        {
            foreach (var item in col)
                Console.WriteLine(item);
        }

    }
}

