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

        private static BinarySearchTreeNode<T> RebalanceAndReturnNewRoot(BinarySearchTreeNode<T> node)
        {
            if (Math.Abs(node.BalanceFactor) <= 1)
            {
                return node.IsRoot ? node : RebalanceAndReturnNewRoot((BinarySearchTreeNode<T>)node.Parent);
            }

            TreeRotater<T> rotater;
            if (node.LeftHeight > node.RightHeight)
            {
                if (node.Left.RightHeight > node.Left.LeftHeight)
                {
                    node.Left = (BinarySearchTreeNode<T>) new LeftRotater<T>().Rotate(node.Left);
                }
                rotater = new RightRotater<T>();
            }
            else
            {
                if (node.Right.LeftHeight > node.Right.RightHeight)
                {
                    node.Right = (BinarySearchTreeNode<T>) new RightRotater<T>().Rotate(node.Right);
                }
                rotater = new LeftRotater<T>();
            }

            return (BinarySearchTreeNode<T>) rotater.Rotate(node);
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
        public void StaysBalancedWith1000RandomInsertions()
        {
            // create a shuffled queue of 1 to 1000
            int[] numbers = new int[1000];
            for (int i = 0; i < 1000; i++) numbers[i] = i;
            IOrderedEnumerable<int> shuffled = numbers.OrderBy(x => Guid.NewGuid());
            Queue<int> queue = new Queue<int>(shuffled);

            AVLBinarySearchTree<int> tree = new AVLBinarySearchTree<int>(queue.Dequeue());
            while (queue.Count > 0)
            {
                tree.Insert(queue.Dequeue());
                Assert.IsTrue(tree.Root.IsBalanced);
            }
        }
    }
}
