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
            Rebalancer<T> rebalancer = new Rebalancer<T>(node);
            this.root = rebalancer.NewRoot();
            return node;
        }

        public class Rebalancer<T> where T : IComparable
        {
            private BinarySearchTreeNode<T> subTreeParent;
            private BinarySearchTreeNode<T> subTreeRoot;
            public BinarySearchTreeNode<T> SubTreeRoot { get { return subTreeRoot; } }

            public Rebalancer(BinarySearchTreeNode<T> subTreeRoot)
            { 
                this.subTreeRoot = subTreeRoot;
                this.subTreeParent = (BinarySearchTreeNode<T>) SubTreeRoot.Parent;
            }

            protected delegate BinarySearchTreeNode<T> RebalanceStrategy(BinarySearchTreeNode<T> child, BinarySearchTreeNode<T> grandChild);

            protected BinarySearchTreeNode<T> DetermineNewRoot(BinarySearchTreeNode<T> otherNode)
            {
                if (subTreeParent == null) return otherNode;

                if (subTreeParent.Right == SubTreeRoot)
                {
                    subTreeParent.Right = otherNode;
                }
                else
                {
                    subTreeParent.Left = otherNode;
                }
                return subTreeParent;
            }

            protected BinarySearchTreeNode<T> RightRightStrategy(BinarySearchTreeNode<T> child, BinarySearchTreeNode<T> grandChild)
            {
                SubTreeRoot.Right = child.Left;
                child.Left = SubTreeRoot;
                return DetermineNewRoot(child);
            }

            protected BinarySearchTreeNode<T> LeftLeftStrategy(BinarySearchTreeNode<T> child, BinarySearchTreeNode<T> grandChild)
            {
                SubTreeRoot.Left = child.Right;
                child.Right = SubTreeRoot;
                return DetermineNewRoot(child);
            }

            protected BinarySearchTreeNode<T> RightLeftStrategy(BinarySearchTreeNode<T> child, BinarySearchTreeNode<T> grandChild)
            {
                child.Left = grandChild.Right;
                SubTreeRoot.Right = grandChild.Left;
                grandChild.Right = child;
                grandChild.Left = SubTreeRoot;
                return DetermineNewRoot(grandChild);
            }

            protected BinarySearchTreeNode<T> LeftRightStrategy(BinarySearchTreeNode<T> child, BinarySearchTreeNode<T> grandChild)
            {
                SubTreeRoot.Left = grandChild.Right;
                child.Right = grandChild.Left;
                grandChild.Left = child;
                grandChild.Right = SubTreeRoot;
                return DetermineNewRoot(grandChild);
            }

            public BinarySearchTreeNode<T> NewRoot()
            {
                BinarySearchTreeNode<T> newRoot = this.SubTreeRoot;
                if (!subTreeRoot.IsBalanced)
                {
                    RebalanceStrategy strategy;
                    BinarySearchTreeNode<T> higherChild = subTreeRoot.HigherChild;
                    BinarySearchTreeNode<T> higherGrandChild = higherChild.HigherChild;

                    if (subTreeRoot.BalanceFactor > 0)
                    {
                        if (higherChild.BalanceFactor > 0) strategy = LeftLeftStrategy;
                        else strategy = LeftRightStrategy;
                    }
                    else
                    {
                        if (higherChild.BalanceFactor > 0) strategy = RightLeftStrategy;
                        else strategy = RightRightStrategy;
                    }

                    newRoot = strategy(higherChild, higherGrandChild);
                }

                if (newRoot.IsRoot) return newRoot;

                Rebalancer<T> parentBalancer = new Rebalancer<T>((BinarySearchTreeNode<T>)newRoot.Parent);
                return parentBalancer.NewRoot();
            }
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
    }
}
