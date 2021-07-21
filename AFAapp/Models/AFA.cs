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
    }
}



