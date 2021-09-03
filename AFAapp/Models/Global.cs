using System;
using System.Collections.Generic;
using System.Linq;

namespace AFAapp.Models
{
    // Helper class that includes common methods and variables shared among the other classes. 
    public static class Global
    {
       
        public static string[] booleans = { "0", "1", "true", "false"};
        public static string[] connectives = { "and", "not", "or" };
        public static string[] connectivesExceptNot = { "and", "or" };

        // Converts a string to array with empty space as splitter.
        public static string[] stringToArray(string s)
        {
            string s1 = s.Replace("(", "( ");
            string s2 = s1.Replace(")", " )");
            string[] subs = s2.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            return subs;
        }

        // Converts a string to a list with parentheses removed.
        public static List<string> stringToListNoPar(string s)
        {
            var subs = stringToArray(s);
            var strArrNoPar = subs.Where(x => x != "(" && x != ")");

            return strArrNoPar.ToList();
        }

        // Prints an IEnumerable to the console.
        public static void Print<T>(IEnumerable<T> col)
        {
            foreach (var item in col)
                Console.WriteLine(item);
        }

    }
}

