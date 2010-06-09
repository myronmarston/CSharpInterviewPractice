using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CSharpInterviewPractice
{
    class BinaryTreeNode<T> : TreeNode<T>
    {
        public BinaryTreeNode(T value) : base(value) { }

        public BinaryTreeNode(T value, BinaryTreeNode<T> parent) : base(value, parent) { }


        private BinaryTreeNode<T> left;
        private BinaryTreeNode<T> right;
        public virtual BinaryTreeNode<T> Left
        {
            get { return left; }
            set 
            {
                if (left != null) left.Parent = null;
                left = value;
                if (value != null) value.Parent = this;
            }
        }
        public virtual BinaryTreeNode<T> Right 
        {
            get { return right; }
            set 
            { 
                if (right != null) right.Parent = null;
                right = value;
                if (value != null) value.Parent = this;
            }
        } 

        public override IList<TreeNode<T>> Children
        {
            get
            {
                return new TreeNode<T>[] { Left, Right }.Where((TreeNode<T> node) => node != null).ToList<TreeNode<T>>();
            }
        }

        public virtual T LeftValue
        {
            get { return (Left == null) ? default(T) : Left.Value; }
            set { Left = new BinaryTreeNode<T>(value); }
        }

        public virtual T RightValue
        {
            get { return (Right == null) ? default(T) : Right.Value; }
            set { Right = new BinaryTreeNode<T>(value); }
        }

        public override TreeNode<T> AddChild(T value)
        {
            throw new InvalidOperationException("AddChild is not supported on a BinaryTreeNode.  Use Left or Right instead.");
        }
    }

    [TestFixture]
    class BinaryTreeNodeTest
    {
        [Test]
        public void settingLeftSetsParent()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            root.Left = new BinaryTreeNode<int>(2);
            BinaryTreeNode<int> origLeft = root.Left;
            Assert.AreSame(root, origLeft.Parent);
            root.Left = null; // should not throw an exception
            Assert.IsNull(origLeft.Parent);
        }

        [Test]
        public void settingRightSetsParent()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            root.Right = new BinaryTreeNode<int>(8);
            BinaryTreeNode<int> origRight = root.Right;
            Assert.AreSame(root, origRight.Parent);
            root.Right = null;
            Assert.IsNull(origRight.Parent);
        }

        [Test]
        public void LeftValueWorksCorrectly()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            Assert.AreEqual(default(int), root.LeftValue);
            root.LeftValue = 7;
            Assert.AreEqual(7, root.LeftValue);
            Assert.AreSame(root, root.Left.Parent);
        }

        [Test]
        public void RightValueWorksCorrectly()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            Assert.AreEqual(default(int), root.RightValue);
            root.RightValue = 7;
            Assert.AreEqual(7, root.RightValue);
            Assert.AreSame(root, root.Right.Parent);
        }

        [Test]
        public void ChildrenReturnsExpectedValues()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            Assert.AreEqual(0, root.Children.Count);

            root.RightValue = 3;
            Assert.AreEqual(1, root.Children.Count);
            Assert.AreEqual(3, root.Children[0].Value);

            root.LeftValue = 14;
            Assert.AreEqual(2, root.Children.Count);
        }

        [Test]
        public void AddChildThrowsException()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            Assert.Throws<InvalidOperationException>(delegate { root.AddChild(7); });
        }
    }
}
