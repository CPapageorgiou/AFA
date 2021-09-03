using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AFAapp.Models
{
    // Responsible for the main functionality of the application.
    // Includes methods for generateting computations step-by-step and creating
    // the Tree object for the computation.

    public class ProgramManager
    {
        // Properties and Fields.
        public string inputWord { get; }
        public AFA afa { get; }

        // Constructors.

        // Object from the input word and the AFA. 
        public ProgramManager(string inputWord, AFA afa)
        {
            this.inputWord = inputWord;
            this.afa = afa;
        }


        // ------------------------------- Generate Computation-Methods -------------------------------


        // Generates the computation step-by-step.
        public (List<Substitution>, Derivation) generateComputation()
        {
            Derivation d = new Derivation(afa.initialState, inputWord[0]);
            var subsList = new List<Substitution>();
            string initial = afa.initialState;
            var keys = afa.getKeys();
            int count = 0;
            int level = 0;
            foreach (char letter in inputWord)
            {
                string initialUpdated = "";
                count += 1;
                foreach (string str in Global.stringToArray(initial))
                {
                    var stateLetter = (str, letter);

                    if (keys.Contains(stateLetter))
                    {
                        initialUpdated += "(" + afa.transitionFunction[stateLetter] + ")";
                        subsList.Add(new Substitution(letter, str, afa.transitionFunction[stateLetter]));
                    }

                    else if (Global.connectives.Contains(str))
                    {
                        if (str != "not")
                        {
                            initialUpdated += " " + str + " ";
                        }

                        else
                        {
                            initialUpdated += str + " ";
                        }
                    }

                    else if (Global.booleans.Contains(str) || str == "(" || str == ")")
                    {
                        initialUpdated += str;
                    }
                }

                if (count < inputWord.Length)
                {
                    d.addStep(initialUpdated, inputWord[count]);
                }

                else
                {
                    d.addStep(initialUpdated, '\0');
                }
                initial = initialUpdated;
                level += 1;
            }
            return (subsList, d);
        }

        // Assigns true to final states and false to non-final states in the last formula of the computation.
        public string statesToBool(string stringToEval, List<string> final)
        {
            string[] arr = Global.stringToArray(stringToEval);
            int ind = 0;
            string newString = "";
            foreach (string str in arr)
            {
                if (final.Contains(str))
                {
                    arr[ind] = "true";
                }

                else if (!Global.connectives.Contains(str) && !Global.booleans.Contains(str) && str != "(" && str != ")")
                {
                    arr[ind] = "false";
                }

                ind += 1;
            }

            foreach (var item in arr)
            {
                newString += item + " ";
            }

            newString.Replace("( ", "(").Replace(" (", "(").Replace(") ", ")").Replace(" )", ")");

            return newString;
        }

        // Evauates the last formula of the computation.
        public static bool computeString(String expression)
        {
            DataTable table = new DataTable();
            return (bool)table.Compute(expression, "");
        }

        // Returns a boolean indicating acceptance or not with the formula evaluated.
        public (string, bool) determineAcceptance()
        {
            var (s, d) = generateComputation();
            string finalString = d.lastString();
            string stringToEval = statesToBool(finalString, afa.finalStates);
            return ((stringToEval, computeString(stringToEval)));
        }


        // ------------------------------- Generate Tree -------------------------------

        // Genretates a Tree object from a computation.
        public Tree generateTree(int currentLevel, Derivation d, List<Substitution> subsList, string parentFormula)
        {
            Tree tree = new Tree();

            if (currentLevel == 0)
            {
                var firstStep = d.getStep(0);
                tree.letter = firstStep.Item2;
                tree.node = firstStep.Item1;
            }

            int n = 0;
            var step = d.getStep(currentLevel);
            var letter = step.Item2;
            var formula = Global.stringToArray(step.Item1);
            //var formulaNoPar = formula.Where(x => x != "(" && x != ")");
            var formulaNoPar = Global.stringToListNoPar(step.Item1);
            var connectivesList = generateConnectivesList(formula);
            var parentStates = statesFromFormula(parentFormula);

            foreach (var con in connectivesList)
            {
                tree.addConnective(con.Item1, con.Item2);
            }

            foreach (var formulaComponent in formulaNoPar)
            {
                if (!Global.connectives.Contains(formulaComponent))
                {
                    foreach (var sub in subsList)
                    {
                        var parentFormulaList = Global.stringToListNoPar(parentFormula);
                        var subParentFormulaLength = parentFormulaList.Count() - n;
                        var subParentFormula = parentFormulaList.GetRange(n, subParentFormulaLength);

                        if (letter == sub.letter && formulaComponent == sub.state && subParentFormula.Contains(formulaComponent))
                        {
                            if (numberOfStates(parentFormula) > n)
                            {
                                if (parentStates[n] == sub.state)
                                {
                                    n += 1;
                                    char childLetter = d.getStep(currentLevel + 1).Item2;

                                    subsList.Remove(sub);

                                    if (currentLevel + 2 == d.length())
                                    {
                                        tree.addChild(new Tree(childLetter, sub.formula, tree.connectives));
                                    }

                                    else
                                    {
                                        tree.addChild(new Tree(childLetter, sub.formula, tree.connectives, new List<Tree> { generateTree(currentLevel + 1, d, subsList, sub.formula) }));
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            tree.removeEmpty();
            return tree;
        }

        // Returns a list of logical connectives where each element is a string
        // of connectives with an integer insicating the child number at which the connectives
        // should be added in the tree.
        public List<(string, int)> generateConnectivesList(string[] strArr)
        {
            var connectivesList = new List<(string, int)>();
            int conInt = -1;

            for (int j = 0; j < strArr.Count(); j++)
            {
                string element = strArr.ElementAt(j);

                if (Global.connectivesExceptNot.Contains(element))
                {
                    if (j == 0 || !Global.connectives.Contains(findNearestNotPar(strArr, j - 1)))
                    {
                        conInt += 1;
                    }
                    connectivesList.Add((element, conInt));
                }

                else if (element == "not")
                {
                    if (strArr.ElementAt(j - 1) == "(")
                    {
                        connectivesList.Add((strArr.ElementAt(j), conInt + 1));
                    }

                    else
                    {
                        connectivesList.Add((strArr.ElementAt(j), conInt));
                    }
                }
            }

            return connectivesList;
        }

        // Returns the nearest element that is not parenthesis.
        public string findNearestNotPar(string[] arr, int ind)
        {
            string s = "";

            for (int i = ind; i >= 0; i--)
            {
                if (arr[i] != "(" && arr[i] != ")")
                {
                    s = arr[i];
                    break;
                }
            }
            return s;
        }

        // Gives the states included in a formula.
        public List<string> statesFromFormula(string formula)
        {
            var states = new List<string>();
            var arr = Global.stringToArray(formula);
            foreach (var item in arr)
            {
                if (!Global.connectives.Contains(item))
                {
                    states.Add(item);
                }
            }
            return states;
        }

        // Returns the number of states in a formula.
        public int numberOfStates(string s)
        {
            int numberOfStates = 0;
            var arr = Global.stringToArray(s);
            foreach (var item in arr)
            {
                if (!Global.connectives.Contains(item))
                {
                    numberOfStates += 1;
                }
            }
            return numberOfStates;
        }

        // Other methods.

        // Checks if the input word is valid.
        public bool inputWordIsValid()
        {
            List<char> letters = afa.getLetters();
            return inputWord.All(c => letters.Contains(c));
        }

    }
}



