using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CSharpInterviewPractice
{
    delegate void VisitNodeDelegate<T>(BinaryTreeNode<T> node);

    interface TreeTraverser<T>
    {
        void Traverse(BinaryTreeNode<T> node, VisitNodeDelegate<T> visit);
    }

    class PreOrderTraverser<T> : TreeTraverser<T>
    {
        public void Traverse(BinaryTreeNode<T> node, VisitNodeDelegate<T> visit)
        {
            visit(node);
            if (node.Left != null) Traverse(node.Left, visit);
            if (node.Right != null) Traverse(node.Right, visit);
        }
    }

    class InOrderTraverser<T> : TreeTraverser<T>
    {
        public void Traverse(BinaryTreeNode<T> node, VisitNodeDelegate<T> visit)
        {
            if (node.Left != null) Traverse(node.Left, visit);
            visit(node);
            if (node.Right != null) Traverse(node.Right, visit);
        }
    }

    class PostOrderTraverser<T> : TreeTraverser<T>
    {
        public void Traverse(BinaryTreeNode<T> node, VisitNodeDelegate<T> visit)
        {
            if (node.Left != null) Traverse(node.Left, visit);
            if (node.Right != null) Traverse(node.Right, visit);
            visit(node);
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

            traverser.Traverse(root, delegate (BinaryTreeNode<char> node)
            {
                actual.Add(node.Value);
            });

            Assert.AreEqual(expected, actual.ToArray<char>());
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
    }
}
