using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CSharpInterviewPractice
{
    class AVLBinarySearchTree<T> where T : IComparable
    {
        private BinarySearchTreeNode<T> root;
        public BinarySearchTreeNode<T> Root { get { return root; } }
        public AVLBinarySearchTree(T value)
        {
            this.root = new BinarySearchTreeNode<T>(value);
        }

        public BinarySearchTreeNode<T> Insert(T value)
        {
            BinarySearchTreeNode<T> node = this.Root.Insert(value);
            this.root = RebalanceAndReturnNewRoot(node);
            return node;
        }

        public BinarySearchTreeNode<T> Delete(T value)
        {
            BinarySearchTreeNode<T> node = this.Root.FindNode(value);
            BinarySearchTreeNode<T> replacementNode = null;

            if (node == null) return null;

            if (!node.IsLeaf)
            {
                replacementNode = RemoveReplacementNode(node);
                replacementNode.Left = node.Left;
                replacementNode.Right = node.Right;
            }

            if (!node.IsRoot)
            {
                if (node.Parent.Left == node) node.Parent.Left = replacementNode;
                else if (node.Parent.Right == node) node.Parent.Right = replacementNode;
            }

            return node;
        }

        private static BinarySearchTreeNode<T> RemoveReplacementNode(BinarySearchTreeNode<T> node)
        {
            if (node.LeftHeight >= node.RightHeight)
            {
                node = node.Left;
                while (node.Right != null) node = node.Right;
                if (node.Left != null) node.Parent.Right = node.Left;
                return node;
            }
            else if (node.RightHeight > node.LeftHeight)
            {
                node = node.Right;
                while (node.Left != null) node = node.Left;
                if (node.Right != null) node.Parent.Left = node.Right;
                return node;
            }
            
            return null;
        }

        private static BinarySearchTreeNode<T> RebalanceAndReturnNewRoot(BinarySearchTreeNode<T> node)
        {
            BinarySearchTreeNode<T> nextNode;

            if (node.BalanceFactor > 1)
            {
                if (node.Left.RightHeight > node.Left.LeftHeight)
                {
                    new LeftRotater<T>(node.Left).Rotate();
                }
                nextNode = (BinarySearchTreeNode<T>)new RightRotater<T>(node).Rotate();
            }
            else if (node.BalanceFactor < -1)
            {
                if (node.Right.LeftHeight > node.Right.RightHeight)
                {
                    new RightRotater<T>(node.Right).Rotate();
                }
                nextNode = (BinarySearchTreeNode<T>)new LeftRotater<T>(node).Rotate();
            }
            else if (node.IsRoot) return node;
            else nextNode = (BinarySearchTreeNode<T>) node.Parent;

            return RebalanceAndReturnNewRoot(nextNode);
        }
    }

    [TestFixture]
    class AVLBinarySearchTreeNodeTest
    {
        [Test]
        public void NoRebalancingExample()
        {
            AVLBinarySearchTree<int> tree = new AVLBinarySearchTree<int>(5);
            tree.Insert(4);
            tree.Insert(7);

            Assert.AreEqual(5, tree.Root.Value);
            Assert.AreEqual(4, tree.Root.LeftValue);
            Assert.AreEqual(7, tree.Root.RightValue);
        }

        [Test]
        public void LeftLeftExample()
        {
            AVLBinarySearchTree<int> tree = new AVLBinarySearchTree<int>(5);
            tree.Insert(3);
            tree.Insert(2);

            Assert.AreEqual(3, tree.Root.Value);
            Assert.AreEqual(2, tree.Root.LeftValue);
            Assert.AreEqual(5, tree.Root.RightValue);
        }

        [Test]
        public void RightRightExample()
        {
            AVLBinarySearchTree<int> tree = new AVLBinarySearchTree<int>(3);
            tree.Insert(5);
            tree.Insert(7);

            Assert.AreEqual(5, tree.Root.Value);
            Assert.AreEqual(3, tree.Root.LeftValue);
            Assert.AreEqual(7, tree.Root.RightValue);
        }

        [Test]
        public void LeftRightExample()
        {
            AVLBinarySearchTree<int> tree = new AVLBinarySearchTree<int>(5);
            tree.Insert(3);
            tree.Insert(4);

            Assert.AreEqual(4, tree.Root.Value);
            Assert.AreEqual(3, tree.Root.LeftValue);
            Assert.AreEqual(5, tree.Root.RightValue);
        }

        [Test]
        public void RightLeftExample()
        {
            AVLBinarySearchTree<int> tree = new AVLBinarySearchTree<int>(3);
            tree.Insert(5);
            tree.Insert(4);

            Assert.AreEqual(4, tree.Root.Value);
            Assert.AreEqual(3, tree.Root.LeftValue);
            Assert.AreEqual(5, tree.Root.RightValue);
        }

        [Test]
        public void SubtreeBalancingExample1()
        {
            AVLBinarySearchTree<int> tree = new AVLBinarySearchTree<int>(3);
            tree.Insert(2);
            tree.Insert(1);
            tree.Insert(5);
            tree.Insert(4);

            Assert.AreEqual(5, tree.Root.NodeCount);
            Assert.AreEqual(2, tree.Root.Value);
            Assert.AreEqual(1, tree.Root.LeftValue);
            Assert.AreEqual(4, tree.Root.RightValue);
            Assert.AreEqual(3, tree.Root.Right.LeftValue);
            Assert.AreEqual(5, tree.Root.Right.RightValue);
        }

        [Test]
        public void SubtreeBalancingExample2()
        {
            AVLBinarySearchTree<int> tree = new AVLBinarySearchTree<int>(1);
            tree.Insert(3);
            tree.Insert(2);
            tree.Insert(4);

            Assert.AreEqual(4, tree.Root.NodeCount);
            Assert.AreEqual(2, tree.Root.Value);
            Assert.AreEqual(1, tree.Root.LeftValue);
            Assert.AreEqual(3, tree.Root.RightValue);
            Assert.AreEqual(4, tree.Root.Right.RightValue);
        }

        [Test]
        public void SubSubTreeBalancingExample()
        {
            AVLBinarySearchTree<int> tree = new AVLBinarySearchTree<int>(5);
            tree.Insert(3); tree.Insert(2); tree.Insert(4); tree.Insert(6);
            tree.Insert(1); tree.Insert(8);
            tree.Insert(7);

            Assert.AreEqual(8, tree.Root.NodeCount);
            Assert.IsTrue(tree.Root.IsBalanced);
        }

        [Test]
        public void StaysBalancedWithRandomInsertionsAndDeletions()
        {
            int nodeCount = 100;
            // create a shuffled queue of 1 to 100
            int[] numbers = new int[nodeCount];
            for (int i = 0; i < nodeCount; i++) numbers[i] = i;
            Queue<int> queue = new Queue<int>(numbers.OrderBy(x => Guid.NewGuid()));

            AVLBinarySearchTree<int> tree = new AVLBinarySearchTree<int>(queue.Dequeue());
            while (queue.Count > 0)
            {
                tree.Insert(queue.Dequeue());
                Assert.AreEqual(nodeCount - queue.Count, tree.Root.NodeCount);
                Assert.IsTrue(tree.Root.IsBalanced);
            }

            int value;
            BinarySearchTreeNode<int> node;
            queue = new Queue<int>(numbers.OrderBy(x => Guid.NewGuid()));
            while (queue.Count > 0)
            {
                value = queue.Dequeue();
                node = tree.Delete(value);
                Assert.AreEqual(value, node.Value);
                Assert.IsNull(tree.Delete(value));
                Assert.AreEqual(queue.Count, tree.Root.NodeCount);
                Assert.IsTrue(tree.Root.IsBalanced);
            }
        }
    }
}
