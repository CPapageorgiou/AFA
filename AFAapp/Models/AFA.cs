using System.Collections.Generic;
using System.Linq;

namespace AFAapp.Models
{

    // Class to represent alternating finite automata. Each instance of this class is an alternating finite automaton.

    public class AFA
    {
        //Properties and Fields.

        public Dictionary<(string, char), string> transitionFunction { get; }
        public List<string> finalStates { get; }
        public string initialState { get; }

        // Constructors.

        // Object from the initial state,, the transition function and the set of final states.
        public AFA(string initialState, Dictionary<(string, char), string> transitionFunction, List<string> finalStates)
        {
            this.initialState = initialState;
            this.transitionFunction = transitionFunction;
            this.finalStates = finalStates;
        }

        // Methods.

        // Get state-lettter pairs from the transition function.
        public Dictionary<(string, char), string>.KeyCollection getKeys()
        {
            return transitionFunction.Keys;
        }

        // Get the set of states.
        public List<string> getStates()
        {
            var states = new List<string>();
            var stateLetterPairs = getKeys();

            foreach (var sateLetter in stateLetterPairs)
            {
                states.Add(sateLetter.Item1);
            }

            return states.Distinct().ToList(); 
        }

        // Get the alphabet.
        public List<char> getLetters()
        {
            var letters = new List<char>();
            var stateLetterPairs = getKeys();

            foreach (var sateLetter in stateLetterPairs)
            {
                letters.Add(sateLetter.Item2);
            }

            return letters.Distinct().ToList();
        }

        // Checks if the initial state is contained in the set of states.
        public bool initialIsValid()
        {
            return getStates().Contains(initialState);
        }

        // Checks if states appear only once in transition function.
        public bool statesAreUnique()
        {
            return getKeys().Distinct().Count() == getKeys().Count();
        }
    }
}



