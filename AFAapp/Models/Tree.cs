using System;
using System.Collections.Generic;
using System.Linq;

namespace AFAapp.Models
{

    public class Tree : IEquatable<Tree>
    {

        public string state { get; set; }

        public List<Tree> children { get; set; }

        public List<(string, int)> connectives { get; set; }

        public char letter { get; set; }


        // Empty
        public Tree()
        {
            this.children = new List<Tree> { };
            this.connectives = new List<(string, int)>();
            this.state = "";
            this.letter = '\0';
        }

        public Tree(string state)
        {
            this.children = new List<Tree> { };
            this.connectives = new List<(string, int)>();
            this.state = this.state = state;
            this.letter = '\0';
        }


        // No children
        public Tree(char letter, string state, List<(string, int)> connectives)
        {
            this.letter = letter;
            this.state = state;
            this.connectives = connectives;
            //children = new List <Tree> { };
        }


        public Tree(char letter, string state, List<(string, int)> connectives, List<Tree> children)
        {

            this.children = children;
            this.connectives = connectives;
            this.state = state;
            this.letter = letter;
        }


        public void removeEmpty()
        {
            int count = 0;

            if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    count += 1;

                    if (children[i].state == "" && children[i].children != null)
                    {
                        children.AddRange(children[i].children);

                        children.RemoveAt(i);
                    }

                }

                for (int i = 0; i < children.Count(); i++)
                {
                    children[i].removeEmpty();
                }
            }
        }




        public void addChild(Tree t)
        {
            children.Add(t);
        }



        public void addConnective(string conn, int n)
        {
            connectives.Add((conn, n));
        }



        public int height(int r = 0)
        {
            if (children == null || children.Count == 0)

            {
                return r;
            }

            else

            {
                r += 1;
                return children[0].height(r);
            }
        }



        public List<(Tree, int)> nLev(int n, int k = 0, List<(Tree, int)> treeList = default)
        {
            if (treeList == default) treeList = new List<(Tree, int)>();

            if (n == 0)
            {
                treeList.Add((this, k));
            }

            else
            {
                if (children != null)
                {
                    for (int j = 0; j < children.Count; j++)
                    {
                        children[j].nLev(n - 1, j, treeList);
                    }
                }
            }
            return treeList;
        }



        public void setConnectives()
        {
            int treeheight = this.height();

            List<(Tree, int)> treeLevels = new List<(Tree, int)>();

            List<(string, int)> y;

            for (int i = 0; i <= treeheight; i++)
            {
                treeLevels = nLev(i);

                int numberOfTrees = treeLevels.Count;

                for (int j = 0; j < numberOfTrees; j++)
                {
                    y = new List<(string, int)>(treeLevels[j].Item1.connectives);
                    y.RemoveAll(r => r.Item2 != j);
                    treeLevels[j].Item1.connectives = new List<(string, int)>(y);

                }
            }

        }



        public void PrintPretty(bool last = true, string indent = "")
        {
            string connectivesStr = "";


            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }

            foreach (var i in connectives)
            {
                connectivesStr += i.Item1 + " ";
            }

            Console.WriteLine($"{state} {connectivesStr} ({letter})");


            for (int i = 0; i < children?.Count; i++)
            {
                children[i].PrintPretty(i == children.Count - 1, indent);
            }

        }



        public override bool Equals(object obj)
        {
            return Equals(obj as Tree);
        }

        public bool Equals(Tree other)
        {
            if (this.children == null && other.children != null)
            {
                return false;

            }

            else if (this.children != null && other.children == null)
            {
                return false;

            }

            else if (this.children == null && other.children == null)
            {
                return state == other.state && letter == other.letter && connectives.SequenceEqual(other.connectives);
            }

            return other != null &&
               state == other.state &&
               children.SequenceEqual(other.children) &&
               //EqualityComparer<List<Tree>>.Default.Equals(children, other.children) &&
               connectives.SequenceEqual(other.connectives)
                //   EqualityComparer<List<string>> .Default.SequenceEquals(connectives, other.connectives)
                && letter == other.letter;

        }

        public override int GetHashCode()
        {
            return HashCode.Combine(state, children, connectives, letter);
        }




        // Unused


        static Tree t = new Tree();

        public Tree previousLevel(Tree node)
        {

            for (int i = 0; i < this.children?.Count(); i++)
            {
                if (this.children[i].Equals(node))
                {
                    t = this;
                    return t;
                }
            }

            for (int i = 0; i < this.children?.Count(); i++)
            {
                this.children[i].previousLevel(node);
            }

            return t;
        }



        public Tree nodesAtLevel(int n)
        {

            if (children.Count == 0)
            {
                return this;
            }

            else if (n == 0)
            {
                return this;
            }

            else
            {
                return children[0].nodesAtLevel(n - 1);
            }
        }



        public bool isRightmost()
        {
            bool b = false;

            int x = children.Count;

            for (int i = 0; i < x; i++)
            {
                children[i].isRightmost();
            }

            return b;
        }



    }
}




