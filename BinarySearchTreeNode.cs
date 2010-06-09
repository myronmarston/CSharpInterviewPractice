using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CSharpInterviewPractice
{
    class BinarySearchTreeNode<T> : BinaryTreeNode<T> where T : IComparable
    {
        public BinarySearchTreeNode(T value) : base(value) { }

        new public BinarySearchTreeNode<T> Left
        {
            get { return (BinarySearchTreeNode<T>) base.Left; }
            set { base.Left = value; }
        }

        new public BinarySearchTreeNode<T> Right
        {
            get { return (BinarySearchTreeNode<T>) base.Right; }
            set { base.Right = value; }
        }

        public override T LeftValue
        {
            get { return base.LeftValue; }
            set { Left = new BinarySearchTreeNode<T>(value); }
        }

        public override T RightValue
        {
            get { return base.RightValue; }
            set { Right = new BinarySearchTreeNode<T>(value); }
        }

        public BinarySearchTreeNode<T> Insert(T value)
        {
            if (value.CompareTo(this.Value) < 0)
            {
                if (Left != null) return Left.Insert(value);
                LeftValue = value;
                return Left;
            }
            else if (value.CompareTo(this.Value) > 0)
            {
                if (Right != null) return Right.Insert(value);
                RightValue = value;
                return Right;
            }
            else
            {
                throw new ArgumentException("Value is alredy present in the tree.", "value");
            }
        }

        public int BalanceFactor { get { return LeftHeight - RightHeight; } }
        private int RightHeight { get { return Right == null ? 0 : Right.Height + 1; } }
        private int LeftHeight { get { return Left == null ? 0 : Left.Height + 1; } }

        public bool IsBalanced { get { return Math.Abs(BalanceFactor) < 2; } }

        public BinarySearchTreeNode<T> HigherChild
        { 
            get
            {
                int bFactor = BalanceFactor;
                if (bFactor == 0) return null;
                else if (bFactor > 0) return Left;
                else return Right;
            }
       }
    }

    [TestFixture]
    class BinarySearchTreeNodeTest
    {
        [Test]
        public void BalancePropertiesReturnExpectedValues()
        {
            BinarySearchTreeNode<int> root = new BinarySearchTreeNode<int>(12);
            Assert.AreEqual(0, root.BalanceFactor);
            Assert.IsTrue(root.IsBalanced);

            root.Insert(8);
            Assert.AreEqual(1, root.BalanceFactor);
            Assert.IsTrue(root.IsBalanced);

            root.Insert(3);
            Assert.AreEqual(2, root.BalanceFactor);
            Assert.IsFalse(root.IsBalanced);
        }

        [Test]
        public void HigherChildReturnsLeftChildIfLeftChildIsHigher()
        {
            BinarySearchTreeNode<int> root = new BinarySearchTreeNode<int>(12);
            root.Insert(5);
            root.Insert(15);
            root.Insert(9);

            Assert.AreSame(root.Left, root.HigherChild);
        }

        [Test]
        public void HigherChildReturnsRightChildIfRightChildIsHigher()
        {
            BinarySearchTreeNode<int> root = new BinarySearchTreeNode<int>(12);
            root.Insert(5);
            root.Insert(15);
            root.Insert(13);

            Assert.AreSame(root.Right, root.HigherChild);
        }

        [Test]
        public void HigherChildReturnsNullIfNeitherChildIsHigher()
        {
            BinarySearchTreeNode<int> root = new BinarySearchTreeNode<int>(12);
            root.Insert(5);
            root.Insert(15);

            Assert.IsNull(root.HigherChild);
        }

        [Test]
        public void InsertRejectsValuesAlreadyPresent()
        {
            BinarySearchTreeNode<int> root = new BinarySearchTreeNode<int>(12);
            Assert.Throws<ArgumentException>(delegate { root.Insert(12); });
        }

        [Test]
        public void InsertKeepsTreeSorted()
        {
            BinarySearchTreeNode<int> root = new BinarySearchTreeNode<int>(12);
            root.Insert(8);
            root.Insert(3);
            root.Insert(15);
            root.Insert(9);
            root.Insert(4);

            Assert.AreEqual(12, root.Value);

            Assert.AreEqual(8, root.LeftValue);
            Assert.AreEqual(15, root.RightValue);

            Assert.AreEqual(3, root.Left.LeftValue);
            Assert.AreEqual(9, root.Left.RightValue);

            Assert.AreEqual(4, root.Left.Left.RightValue);

            Assert.IsNull(root.Right.Left);
            Assert.IsNull(root.Right.Right);

            Assert.IsNull(root.Left.Right.Left);
            Assert.IsNull(root.Left.Right.Right);

            Assert.IsNull(root.Left.Left.Left);
            Assert.IsNull(root.Left.Left.Right.Left);
            Assert.IsNull(root.Left.Left.Right.Right);            
        }
    }
}
