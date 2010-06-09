using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CSharpInterviewPractice
{
    delegate bool VisitNodeDelegate<T>(BinaryTreeNode<T> node);

    interface TreeTraverser<T>
    {
        BinaryTreeNode<T> Traverse(BinaryTreeNode<T> node, VisitNodeDelegate<T> visit);
    }

    class PreOrderTraverser<T> : TreeTraverser<T>
    {
        public BinaryTreeNode<T> Traverse(BinaryTreeNode<T> node, VisitNodeDelegate<T> visit)
        {
            BinaryTreeNode<T> returnedNode; 

            if (visit(node)) return node;
            if (node.Left != null)
            {
                returnedNode = Traverse(node.Left, visit);
                if (returnedNode != null) return returnedNode;
            }

            if (node.Right != null) return Traverse(node.Right, visit);

            return null;
        }
    }

    class InOrderTraverser<T> : TreeTraverser<T>
    {
        public BinaryTreeNode<T> Traverse(BinaryTreeNode<T> node, VisitNodeDelegate<T> visit)
        {
            BinaryTreeNode<T> returnedNode;
            if (node.Left != null)
            {
                returnedNode = Traverse(node.Left, visit);
                if (returnedNode != null) return returnedNode;
            }
            if (visit(node)) return node;
            if (node.Right != null) return Traverse(node.Right, visit);
            return null;
        }
    }

    class PostOrderTraverser<T> : TreeTraverser<T>
    {
        public BinaryTreeNode<T> Traverse(BinaryTreeNode<T> node, VisitNodeDelegate<T> visit)
        {
            BinaryTreeNode<T> returnedNode;
            if (node.Left != null)
            {
                returnedNode = Traverse(node.Left, visit);
                if (returnedNode != null) return returnedNode;
            }

            if (node.Right != null)
            {
                returnedNode = Traverse(node.Right, visit);
                if (returnedNode != null) return returnedNode;
            }

            if (visit(node)) return node;
            return null;
        }
    }

    class BreadthFirstTraverser<T> : TreeTraverser<T>
    {
        private Queue<BinaryTreeNode<T>> unvisitedNodes = new Queue<BinaryTreeNode<T>>();
        public BinaryTreeNode<T> Traverse(BinaryTreeNode<T> node, VisitNodeDelegate<T> visit)
        {
            BinaryTreeNode<T> unvisitedNode;
            Queue<BinaryTreeNode<T>> unvisitedNodes = new Queue<BinaryTreeNode<T>>();
            unvisitedNodes.Enqueue(node);

            while (unvisitedNodes.Count > 0)
            {
                unvisitedNode = unvisitedNodes.Dequeue();
                if (visit(unvisitedNode)) return unvisitedNode;

                foreach (BinaryTreeNode<T> childNode in unvisitedNode.Children) unvisitedNodes.Enqueue(childNode);
            }

            return null;
        }
    }

    [TestFixture]
    class TreeTraversersTest
    {
        private BinaryTreeNode<char> ConstructTree()
        {
            BinaryTreeNode<char> root = new BinaryTreeNode<char>('F');

            root.LeftValue = 'B'; root.RightValue = 'G';
            root.Left.LeftValue = 'A'; root.Left.RightValue = 'D';
            root.Left.Right.LeftValue = 'C'; root.Left.Right.RightValue = 'E';
            
            root.Right.RightValue = 'I';
            root.Right.Right.LeftValue = 'H';
            return root;
        }

        private void PerformTest(TreeTraverser<char> traverser, char[] expected)
        {
            BinaryTreeNode<char> root = ConstructTree();
            List<char> actual = new List<char>();

            BinaryTreeNode<char> returnedNode = traverser.Traverse(root, delegate(BinaryTreeNode<char> node)
            {
                actual.Add(node.Value);
                return false;
            });

            Assert.AreEqual(expected, actual.ToArray<char>());
            Assert.IsNull(returnedNode);

            // make sure it exits early and returns the node
            int count = 0;
            returnedNode = traverser.Traverse(root, delegate(BinaryTreeNode<char> node)
            {
                if (count == 3) return true;
                count++;
                return false;
            });

            Assert.AreEqual(expected[3], returnedNode.Value);
            Assert.AreEqual(3, count);
        }

        [Test]
        public void PreOrderTest()
        {
            PerformTest(new PreOrderTraverser<char>(), new char[]{ 'F', 'B', 'A', 'D', 'C', 'E', 'G', 'I', 'H' });
        }

        [Test]
        public void InOrderTest()
        {
            PerformTest(new InOrderTraverser<char>(), new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' });
        }

        [Test]
        public void PostOrderTest()
        {
            PerformTest(new PostOrderTraverser<char>(), new char[] { 'A', 'C', 'E', 'D', 'B', 'H', 'I', 'G', 'F' });
        }

        [Test]
        public void BreadthFirstTest()
        {
            PerformTest(new BreadthFirstTraverser<char>(), new char[] { 'F', 'B', 'G', 'A', 'D', 'I', 'C', 'E', 'H' });
        }
    }
}
