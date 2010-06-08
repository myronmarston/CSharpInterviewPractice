using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CSharpInterviewPractice
{
    class BinaryTreeNode<T> : TreeNode<T> where T : IComparable
    {
        public BinaryTreeNode(T value)
            : base(value) { }

        public BinaryTreeNode(T value, BinaryTreeNode<T> parent)
            : base(value, parent) { }

        public BinaryTreeNode<T> Left { get; set; }
        public BinaryTreeNode<T> Right { get; set; } 

        public override IList<TreeNode<T>> Children
        {
            get
            {
                return new TreeNode<T>[] { Left, Right }.Where((TreeNode<T> node) => node != null).ToList<TreeNode<T>>();
            }
        }

        public T LeftValue
        {
            get { return (Left == null) ? default(T) : Left.Value; }
            set { Left = new BinaryTreeNode<T>(value, this); }
        }

        public T RightValue
        {
            get { return (Right == null) ? default(T) : Right.Value; }
            set { Right = new BinaryTreeNode<T>(value, this); }
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
        public void LeftValueWorksCorrectly()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            Assert.AreEqual(default(int), root.LeftValue);
            root.LeftValue = 7;
            Assert.AreEqual(7, root.LeftValue);
        }

        [Test]
        public void RightValueWorksCorrectly()
        {
            BinaryTreeNode<int> root = new BinaryTreeNode<int>(5);
            Assert.AreEqual(default(int), root.RightValue);
            root.RightValue = 7;
            Assert.AreEqual(7, root.RightValue);
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
