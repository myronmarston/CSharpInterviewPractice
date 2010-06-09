using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CSharpInterviewPractice
{
    interface TreeRotater<T>
    {
        BinaryTreeNode<T> Rotate(BinaryTreeNode<T> root);
    }

    class LeftRotater<T> : TreeRotater<T>
    {
        public BinaryTreeNode<T> Rotate(BinaryTreeNode<T> root)
        {
            if (root == null) throw new ArgumentNullException("root");
            BinaryTreeNode<T> pivot = root.Right;
            if (pivot == null) throw new ArgumentException("No pivot could be found.", "root");

            root.Right = pivot.Left;
            pivot.Left = root;
            return pivot;
        }
    }

    class RightRotater<T> : TreeRotater<T>
    {
        public BinaryTreeNode<T> Rotate(BinaryTreeNode<T> root)
        {
            if (root == null) throw new ArgumentNullException("root");
            BinaryTreeNode<T> pivot = root.Left;
            if (pivot == null) throw new ArgumentException("No pivot could be found.", "root");

            root.Left = pivot.Right;
            pivot.Right = root;
            return pivot;
        }
    }

    [TestFixture]
    class TreeRotatersTest
    {
        [Test]
        public void RotateLeftFromRR()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            root.RightValue = 7; root.Right.RightValue = 9;

            TreeRotater<int> tr = new LeftRotater<int>();
            BinaryTreeNode<int> newRoot = tr.Rotate(root);

            Assert.AreEqual(7, newRoot.Value);
            Assert.AreEqual(5, newRoot.LeftValue);
            Assert.AreEqual(9, newRoot.RightValue);
        }

        [Test]
        public void RotateLeftFromR()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            root.RightValue = 7;

            TreeRotater<int> tr = new LeftRotater<int>();
            BinaryTreeNode<int> newRoot = tr.Rotate(root);

            Assert.AreEqual(7, newRoot.Value);
            Assert.AreEqual(5, newRoot.LeftValue);
        }

        [Test]
        public void RotateLeftFromL()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            root.LeftValue = 3;

            TreeRotater<int> tr = new LeftRotater<int>();
            Assert.Throws<ArgumentException>(delegate { tr.Rotate(root); });
        }

        [Test]
        public void RotateLeftFromNull()
        {
            TreeRotater<int> tr = new LeftRotater<int>();
            Assert.Throws<ArgumentNullException>(delegate { tr.Rotate(null); });
        }

        [Test]
        public void RotateRightFromLL()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            root.LeftValue = 3; root.Left.LeftValue = 2;

            TreeRotater<int> tr = new RightRotater<int>();
            BinaryTreeNode<int> newRoot = tr.Rotate(root);

            Assert.AreEqual(3, newRoot.Value);
            Assert.AreEqual(2, newRoot.LeftValue);
            Assert.AreEqual(5, newRoot.RightValue);
        }

        [Test]
        public void RotateRightFromL()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            root.LeftValue = 3;

            TreeRotater<int> tr = new RightRotater<int>();
            BinaryTreeNode<int> newRoot = tr.Rotate(root);

            Assert.AreEqual(3, newRoot.Value);
            Assert.AreEqual(5, newRoot.RightValue);
        }

        [Test]
        public void RotateRightFromR()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            root.RightValue = 3;

            TreeRotater<int> tr = new RightRotater<int>();
            Assert.Throws<ArgumentException>(delegate { tr.Rotate(root); });
        }

        [Test]
        public void RotateRightFromNull()
        {
            TreeRotater<int> tr = new RightRotater<int>();
            Assert.Throws<ArgumentNullException>(delegate { tr.Rotate(null); });
        }
    }
}
