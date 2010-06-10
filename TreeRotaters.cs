using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CSharpInterviewPractice
{
    abstract class TreeRotater<T>
    {
        protected BinaryTreeNode<T> rootParent;
        protected BinaryTreeNode<T> root;
        protected BinaryTreeNode<T> pivot;
        public TreeRotater(BinaryTreeNode<T> root)
        {
            this.root = root;
            if (root == null) throw new ArgumentNullException("root");
            this.rootParent = (BinaryTreeNode<T>) root.Parent;
            this.pivot = RootOtherSideNode;
            if (pivot == null) throw new ArgumentException("No pivot could be found.", "root");
        }

        protected abstract BinaryTreeNode<T> PivotRotationSideNode { get; set; }
        protected abstract BinaryTreeNode<T> RootOtherSideNode { get; set; }

        public BinaryTreeNode<T> Rotate()
        {
            if (rootParent == null) { }
            else if (root == rootParent.Left) rootParent.Left = pivot;
            else if (root == rootParent.Right) rootParent.Right = pivot;

            RootOtherSideNode = PivotRotationSideNode;
            PivotRotationSideNode = root;
            return pivot;
        }
    }

    class LeftRotater<T> : TreeRotater<T>
    {
        public LeftRotater(BinaryTreeNode<T> root) : base(root) { }

        protected override BinaryTreeNode<T> PivotRotationSideNode
        {
            get { return this.pivot.Left; }
            set { this.pivot.Left = value; }
        }

        protected override BinaryTreeNode<T> RootOtherSideNode
        {
            get { return this.root.Right; }
            set { this.root.Right = value; }
        }
    }

    class RightRotater<T> : TreeRotater<T>
    {
        public RightRotater(BinaryTreeNode<T> root) : base(root) { }

        protected override BinaryTreeNode<T> PivotRotationSideNode
        {
            get { return this.pivot.Right; }
            set { this.pivot.Right = value; }
        }

        protected override BinaryTreeNode<T> RootOtherSideNode
        {
            get { return this.root.Left; }
            set { this.root.Left = value; }
        }
    }

    [TestFixture]
    class TreeRotatersTest
    {
        [Test]
        public void RotateLeftFromRR()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(3);
            root.RightValue = 5; root.Right.RightValue = 7; root.Right.Right.RightValue = 9;

            BinaryTreeNode<int> subTreeRoot = root.Right;

            TreeRotater<int> tr = new LeftRotater<int>(subTreeRoot);
            BinaryTreeNode<int> newSubTreeRoot = tr.Rotate();

            Assert.AreEqual(7, newSubTreeRoot.Value);
            Assert.AreEqual(5, newSubTreeRoot.LeftValue);
            Assert.AreEqual(9, newSubTreeRoot.RightValue);
            Assert.AreSame(root, newSubTreeRoot.Parent);
        }

        [Test]
        public void RotateLeftFromR()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(3);
            root.RightValue = 5; root.Right.RightValue = 7;

            BinaryTreeNode<int> subTreeRoot = root.Right;

            TreeRotater<int> tr = new LeftRotater<int>(subTreeRoot);
            BinaryTreeNode<int> newSubTreeRoot = tr.Rotate();

            Assert.AreEqual(7, newSubTreeRoot.Value);
            Assert.AreEqual(5, newSubTreeRoot.LeftValue);
            Assert.AreSame(root, newSubTreeRoot.Parent);
        }

        [Test]
        public void RotateLeftFromL()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            root.LeftValue = 3;

            Assert.Throws<ArgumentException>(delegate { new LeftRotater<int>(root); });
        }

        [Test]
        public void RotateLeftFromNull()
        {
            Assert.Throws<ArgumentNullException>(delegate { new LeftRotater<int>(null); });
        }

        [Test]
        public void RotateRightFromLL()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(7);
            root.LeftValue = 5; root.Left.LeftValue = 3; root.Left.Left.LeftValue = 2;

            BinaryTreeNode<int> subTreeRoot = root.Left;

            TreeRotater<int> tr = new RightRotater<int>(subTreeRoot);
            BinaryTreeNode<int> newSubTreeRoot = tr.Rotate();

            Assert.AreEqual(3, newSubTreeRoot.Value);
            Assert.AreEqual(2, newSubTreeRoot.LeftValue);
            Assert.AreEqual(5, newSubTreeRoot.RightValue);
            Assert.AreSame(root, newSubTreeRoot.Parent);
        }

        [Test]
        public void RotateRightFromL()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(7);
            root.LeftValue = 5; root.Left.LeftValue = 3;

            BinaryTreeNode<int> subTreeRoot = root.Left;

            TreeRotater<int> tr = new RightRotater<int>(subTreeRoot);
            BinaryTreeNode<int> newSubTreeRoot = tr.Rotate();

            Assert.AreEqual(3, newSubTreeRoot.Value);
            Assert.AreEqual(5, newSubTreeRoot.RightValue);
            Assert.AreSame(root, newSubTreeRoot.Parent);
        }

        [Test]
        public void RotateRightFromR()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            root.RightValue = 3;

            Assert.Throws<ArgumentException>(delegate { new RightRotater<int>(root); });
        }

        [Test]
        public void RotateRightFromNull()
        {
            Assert.Throws<ArgumentNullException>(delegate { new RightRotater<int>(null); });
        }
    }
}
