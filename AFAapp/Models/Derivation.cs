using System;
using System.Collections.Generic;
using System.Linq;

namespace AFAapp.Models
{

    public class Derivation
    {
        List<(string, char)> derivation = new List<(string, char)>();

        public Derivation(List<(string, char)> l)
        {
            derivation = l;
        }


        public Derivation(string initialState, char letter)
        {
            derivation.Add((initialState, letter));
        }


        public (string, char) getStep(int n)
        {
            return derivation[n];
        }


        public string lastString()
        {
            return derivation[derivation.Count - 1].Item1;
        }


        public void addStep(string str, char letter)
        {
            if (letter == '\0')
            {
                derivation.Add((str, '\0'));
            }

            else
            {
                derivation.Add((str, letter));
            }
        }


        public Derivation removeStep((string str, char letter) tup)
        {

            var clonedList = derivation;
            clonedList.Remove(tup);
            return new Derivation(clonedList);
        }

        public int length()
        {
            return derivation.Count;
        }


        public bool isEmpty()
        {

            return derivation.Count == 0;
        }


        public override string ToString()
        {
            string toShow = "";

            foreach ((string i, char l) in derivation)
            {
                if (toShow == "")
                {
                    toShow = "(" + i + "," + l + ")";
                }

                else
                {
                    toShow = toShow + ", " + "(" + i + "," + l + ")";
                }
            }
            return toShow;
        }
    }
}


