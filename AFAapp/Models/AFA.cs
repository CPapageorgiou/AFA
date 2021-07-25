using System;
using System.Collections.Generic;
namespace AFAapp.Models
{

    public class AFA
    {

        public Dictionary<(string, char), string> transitionFunction { get; }
        public List<string> finalStates;
        public string initialState { get; }

        public AFA(string initialState, Dictionary<(string, char), string> transitionFunction, List<string> finalStates)
        {
            this.initialState = initialState;
            this.transitionFunction = transitionFunction;
            this.finalStates = finalStates;

        }

        public Dictionary<(string, char), string>.KeyCollection getKeys()
        {
            return transitionFunction.Keys;
        }


        public List<string> getStates()
        {
            var states = new List<string>();
            var keys = this.getKeys();
            foreach (var key in keys)
            {
                states.Add(key.Item1);
            }
            return states; 
        }

        public List<char> getLetters()
        {
            var letters = new List<char>();
            var keys = this.getKeys();
            foreach (var key in keys)
            {
                letters.Add(key.Item2);
            }
            return letters;
        }



        public bool initialIsValid()
        {
            return (this.getStates().Contains(initialState));
        }
    }
}



