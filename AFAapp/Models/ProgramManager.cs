using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace AFAapp.Models
{

    public class ProgramManager
    {
        public bool determineAcceptance(string inputWord, AFA a)
        {
            var (s, d) = activateAut(inputWord, a);
            string finalString = d.lastString();

            string stringToEval = statesToBool(finalString, a.finalStates);
            return computeString(stringToEval);
        }


        public string statesToBool(string stringToEval, List<string> final)
        {

            foreach (string s in Global.stringToArray(stringToEval))
            {
                //if (Array.Exists(final, element => element == s))
                if(final.Contains(s))
                {
                    stringToEval = stringToEval.Replace(s, "true");
                }

                else if (!Global.connectives.Contains(s) && !Global.booleans.Contains(s) &&  s != "(" && s != ")")
                {
                    stringToEval = stringToEval.Replace(s, "false");
                }
                //else if (!Array.Exists(Global.connectives, element => element == s) && !Global.booleans.Contains(s) && s != "(" && s != ")") // or using System.Linq and then Contains;
                //{
                //    stringToEval = stringToEval.Replace(s, "false");
                //}
            }
            return stringToEval;
        }



        public static bool computeString(String expression)
        {
            DataTable table = new DataTable();
            return (bool)table.Compute(expression, "");
        }


        public (List<(int level, char letter, (string state, string substitution))>, Derivation) activateAut(string inputWord, AFA a)
        {
            int count = 0;
            Derivation d = new Derivation(a.initialState, inputWord[0]);
            var subsList = new List<(int level, char letter, (string state, string substitution))>();
            int level = 0;
            string initial = a.initialState;

            foreach (char letter in inputWord)
            {
                string initialUpdated = "";
                count += 1;

                foreach (string j in Global.stringToArray(initial))
                {
                    var stateLetter = (j, letter);

                    if (a.getKeys().Contains(stateLetter))
                    {
                        initialUpdated += "(" + a.transitionFunction[stateLetter] + ")";
                        subsList.Add((level, letter, (j, a.transitionFunction[stateLetter])));
                    }

                    else if (Global.connectives.Contains(j))
                    {
                        if (j != "not")
                        {
                            initialUpdated += " " + j + " ";
                        }

                        if (j == "not")
                        {
                            initialUpdated += j + " ";
                        }
                    }

                    else if (Global.booleans.Contains(j))
                    {
                        initialUpdated += j;
                    }

                    else if (j == "(" || j == ")")
                    {
                        initialUpdated += j;
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



        public List<(string, int)> generateConnectivesList(string[] strArr)
        {

            var connectivesList = new List<(string, int)>();
            int conInt = -1;

            var strArrNoPar = strArr.Where(x => x != "(" && x != ")");

            for (int i = 0; i < strArrNoPar.Count(); i++)
            {
                if (Global.connectives.Contains(strArrNoPar.ElementAt(i)))
                {

                    if (!Global.connectives.Contains(strArrNoPar.ElementAt(i - 1)))
                    {
                        conInt += 1;
                    }

                    connectivesList.Add((strArrNoPar.ElementAt(i), conInt));
                }
            }

            return connectivesList;
        }



        public Tree generateTree(int level, Derivation d, List<(int level, char letter, (string state, string substitution))> subsList)
        {
            Tree tree = new Tree();

            if (level == 0)
            {
                var firstStep = d.getStep(0);
                tree.letter = firstStep.Item2;
                tree.state = firstStep.Item1;
            }

            int count = 0;

            var step = d.getStep(level);

            int n = 0;


            var strArr = Global.stringToArray(step.Item1);


            var connectivesList = generateConnectivesList(strArr);

            foreach (var con in connectivesList)
            {
                tree.addConnective(con.Item1, con.Item2);

            }

            var strArrNoPar = strArr.Where(x => x != "(" && x != ")");
            var conList = new List<string>();

            foreach (var j in strArrNoPar)
            {

                if (level != 1 && n > 1)
                {
                    break;
                }


                if (!Global.connectives.Contains(j))
                {

                    foreach (var i in subsList)
                    {

                        if (step.Item2 == i.Item2 && j == i.Item3.Item1)
                        {

                            n += 1;

                            level = i.Item1;

                            char m = d.getStep(level + 1).Item2;

                            subsList.Remove(i);


                            if (level + 2 == d.length())
                            {
                                //z += 1;
                                tree.addChild(new Tree(m, i.Item3.Item2, tree.connectives));
                            }

                            else
                            {
                                tree.addChild(new Tree(m, i.Item3.Item2, tree.connectives, new List<Tree> { generateTree(level + 1, d, subsList) }));
                            }

                            count += 1;
                            break;
                        }
                    }
                }
            }
            tree.removeEmpty();
            return tree;
        }
    }
}



