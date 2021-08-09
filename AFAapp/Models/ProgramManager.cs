using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace AFAapp.Models
{

    public class ProgramManager
    {
        public string inputWord { get; set; }
        public AFA a { get; set; }

        public ProgramManager(string inputWord, AFA a)
        {
            this.inputWord = inputWord;
            this.a = a;
        }

        public bool inputWordIsValid()
        {
            List<char> letters = a.getLetters();
            return (inputWord.All(c => letters.Contains(c)));
        }


        public (string, bool) determineAcceptance()
        {
            var (s, d) = activateAut();
            string finalString = d.lastString();
            string stringToEval = statesToBool(finalString, a.finalStates);
            return ((stringToEval, computeString(stringToEval)));
        }


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



        public static bool computeString(String expression)
        {
            DataTable table = new DataTable();
            return (bool)table.Compute(expression, "");
        }


        public (List<(int level, char letter, (string state, string substitution))>, Derivation) activateAut()
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
            Console.WriteLine(d);
            return (subsList, d);
        }



        public List<(string, int)> generateConnectivesList2(string[] strArr)
        {

            var connectivesList = new List<(string, int)>();
            int conInt = -1;

            var strArrNoPar = strArr.Where(x => x != "(" && x != ")");

            for (int i = 0; i < strArrNoPar.Count(); i++)
            {
                if (Global.connectives.Contains(strArrNoPar.ElementAt(i)))
                {

                    if (i == 0 || !Global.connectives.Contains(strArrNoPar.ElementAt(i - 1)))
                    {
                        conInt += 1;
                    }

                    connectivesList.Add((strArrNoPar.ElementAt(i), conInt));
                }
            }

            return connectivesList;
        }



        public List<(string, int)> generateConnectivesList(string[] strArr)
        {

            var connectivesList = new List<(string, int)>();
            int conInt2 = -1;

            for (int j = 0; j < strArr.Count(); j++)
            {
                string element = strArr.ElementAt(j);


                if (Global.connectivesExceptNot.Contains(element))
                {

                    if (j == 0 || !Global.connectives.Contains(findNearestNotPar(strArr, j - 1)))
                    {
                        conInt2 += 1;
                    }
                    connectivesList.Add((element, conInt2));
                }

                else if (element == "not")
                {
                    if (strArr.ElementAt(j - 1) == "(")
                    {
                        connectivesList.Add((strArr.ElementAt(j), conInt2 + 1));
                    }

                    else
                    {
                        connectivesList.Add((strArr.ElementAt(j), conInt2));
                    }
                }
            }

            return connectivesList;
        }


        public string findNearestNotPar(string[] arr, int ind)
        {
            string s = "";

            try
            {
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
            catch
            {
                Console.WriteLine("error");
                return "abc";
            }
        }



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


        public Tree generateTree(int level, Derivation d, List<(int level, char letter, (string state, string substitution))> subsList, string s)
        {
            Tree tree = new Tree();

            if (level == 0)
            {
                var firstStep = d.getStep(0);
                tree.letter = firstStep.Item2;
                tree.state = firstStep.Item1;
            }

            var step = d.getStep(level);

            var strArr = Global.stringToArray(step.Item1);

            var connectivesList = generateConnectivesList(strArr);

            foreach (var con in connectivesList)
            {
                tree.addConnective(con.Item1, con.Item2);
            }

            var strArrNoPar = strArr.Where(x => x != "(" && x != ")");
            var conList = new List<string>();
            int n = 0;

            foreach (var j in strArrNoPar)
            {

                if (!Global.connectives.Contains(j))
                {

                    foreach (var i in subsList)
                    {

                        if (step.Item2 == i.Item2 && j == i.Item3.Item1 && Global.stringToArray(s.Substring(n)).Contains(j))
                        {
                            n += 1;
                            char m = d.getStep(level + 1).Item2;

                            if (numberOfStates(s) > tree.children.Count)
                            {
                                subsList.Remove(i);
                                if (level + 2 == d.length())
                                {
                                    tree.addChild(new Tree(m, i.Item3.Item2, tree.connectives));
                                }

                                else
                                {
                                    tree.addChild(new Tree(m, i.Item3.Item2, tree.connectives, new List<Tree> { generateTree(level + 1, d, subsList, i.Item3.Item2) }));
                                }
                            }
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



