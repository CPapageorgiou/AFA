using System.Collections.Generic;

namespace AFAapp.Models
{

    public class Derivation
    {
        //`Properties and Fields.

        private List<(string, char)> derivation;

        // Constructors.

        // Object from a list.
        public Derivation(List<(string, char)> derivation)
        {
            this.derivation = derivation;
        }

        // Object from the initial state and the first input letter.
        public Derivation(string initialState, char letter)
        {
            derivation = new List<(string, char)>();
            derivation.Add((initialState, letter));
        }

        // Methods.

        // Gets a step of the computation based on its index.
        public (string, char) getStep(int n)
        {
            return derivation[n];
        }

        // Get the final formula of the computation.
        public string lastString()
        {
            return derivation[derivation.Count - 1].Item1;
        }

        // Adds a step to the computation.
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

        // Removes a step from the computation.
        public Derivation removeStep((string str, char letter) tup)
        {

            var clonedList = derivation;
            clonedList.Remove(tup);
            return new Derivation(clonedList);
        }

        // Returns the length of the computation.
        public int length()
        {
            return derivation.Count;
        }

        // Checks if the computation is empty.
        public bool isEmpty()
        {

            return derivation.Count == 0;
        }

        // Prints to console the derivation for testing purposes.
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


