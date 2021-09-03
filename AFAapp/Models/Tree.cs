using System;
using System.Collections.Generic;
using System.Linq;

namespace AFAapp.Models
{
    // Class to create tree objects. Used as the model for the trees.
    public class Tree : IEquatable<Tree>
    {
        // Properties and Fields.

        public string node { get; set; }
        public List<Tree> children { get; set; }
        public List<(string, int)> connectives { get; set; }
        public char letter { get; set; }

        // Constructors.

        // Empty
        public Tree()
        {
            this.children = new List<Tree> { };
            this.connectives = new List<(string, int)>();
            this.node = "";
            this.letter = '\0';
        }

        // Only one state.
        public Tree(string node)
        {
            this.children = new List<Tree> { };
            this.connectives = new List<(string, int)>();
            this.node = node;
            this.letter = '\0';
        }


        // No children
        public Tree(char letter, string node, List<(string, int)> connectives)
        {
            this.letter = letter;
            this.node = node;
            this.connectives = connectives;
        }

        // Object from the letter the node, the connectives preserved from the previous node and
        // the children of the tree.
        public Tree(char letter, string node, List<(string, int)> connectives, List<Tree> children)
        {

            this.children = children;
            this.connectives = connectives;
            this.node = node;
            this.letter = letter;
        }


        // Methods.

        // Adds a child to the current tree.
        public void addChild(Tree t)
        {
            children.Add(t);
        }

        // Adds a logical connective to the current tree.
        public void addConnective(string conn, int n)
        {
            connectives.Add((conn, n));
        }


        // Returns the zero-based height of a tree.
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

        // Given an integer n it reuturns the list of sub-trees begining from level n.
        // Each sub-tree is associted by an integer indicating which number of child of its parent is. 
        // The length of the returned list is thus the number of nodes at level n.
        public List<(Tree, int)> subTreesAt(int n, int k = 0, List<(Tree, int)> treeList = default)
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
                        children[j].subTreesAt(n - 1, j, treeList);
                    }
                }
            }
            return treeList;
        }


        // Helper method to set the connectives associated to eaach node correctly.
        public void setConnectives()
        {
            int treeHeight = this.height();

            List<(Tree, int)> subTrees = new List<(Tree, int)>();

            List<(string, int)> y;

            for (int i = 0; i <= treeHeight; i++)
            {
                subTrees = subTreesAt(i);

                int numberOfNodes = subTrees.Count;

                for (int j = 0; j < numberOfNodes; j++)
                {
                    y = new List<(string, int)>(subTrees[j].Item1.connectives);
                    y.RemoveAll(r => r.Item2 != j);
                    subTrees[j].Item1.connectives = new List<(string, int)>(y);
                }
            }
        }


        // Removes empty nodes from the tree.
        public void removeEmpty()
        {
            int count = 0;

            if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    count += 1;

                    if (children[i].node == "" && children[i].children != null)
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


        // Prints a tree to the console for testing purposes.
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

            Console.WriteLine($"{node} {connectivesStr} ({letter})");


            for (int i = 0; i < children?.Count; i++)
            {
                children[i].PrintPretty(i == children.Count - 1, indent);
            }

        }


        // Tree equality.

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
                return node == other.node && letter == other.letter && connectives.SequenceEqual(other.connectives);
            }

            return other != null &&
               node == other.node &&
               children.SequenceEqual(other.children) &&
               connectives.SequenceEqual(other.connectives)
                && letter == other.letter;

        }

        public override int GetHashCode()
        {
            return HashCode.Combine(node, children, connectives, letter);
        }

    }
}




