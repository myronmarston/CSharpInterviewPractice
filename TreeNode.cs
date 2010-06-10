using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CSharpInterviewPractice
{
    public class TreeNode<T>
    {
        public TreeNode(T value)
        {
            this.value = value;
        }

        public TreeNode(T value, TreeNode<T> parent)
            : this(value)
        {
            this.parent = parent;
        }

        protected T value;
        protected TreeNode<T> parent;
        protected IList<TreeNode<T>> children;

        public T Value { get { return value; } } 
        public TreeNode<T> Parent 
        { 
            get { return parent;  }
            protected set { parent = value; } 
        }

        public virtual IList<TreeNode<T>> Children { get { if (children == null) children = new List<TreeNode<T>>(); return children; } }

        public TreeNode(TreeNode<T> parent)
        {
            this.parent = parent;
        }

        public bool IsRoot { get { return (Parent == null); } }
        public bool IsLeaf { get { return (Children.Count == 0); } }

        public int Height
        {
            get
            {
                if (IsLeaf) return 0;
                return Children.Max((TreeNode<T> node) => node.Height) + 1;
            }
        }

        public int Depth
        {
            get 
            {
                if (IsRoot) return 0;
                return Parent.Depth + 1;
            }
        }

        public IList<TreeNode<T>> Siblings
        {
            get
            {
                if (this.Parent == null) return new List<TreeNode<T>>();
                return new List<TreeNode<T>>(this.Parent.Children.Except(new TreeNode<T>[] { this }));
            }
        }

        public virtual TreeNode<T> AddChild(T value)
        {
            TreeNode<T> child = new TreeNode<T>(value, this);
            this.Children.Add(child);
            return child;
        }

        public int NodeCount
        {
            get { return Children.Sum(n => n.NodeCount) + 1; }
        }
    }

    [TestFixture]
    class TreeNodeTest
    {
        private TreeNode<int> ExampleTreeRoot()
        {
            TreeNode<int> root = new TreeNode<int>(7);
            TreeNode<int> three = root.AddChild(3);
            three.AddChild(2);
            three.AddChild(5);
            three.AddChild(12);

            TreeNode<int> four = root.AddChild(4);
            TreeNode<int> nine = four.AddChild(9);
            nine.AddChild(6);

            return root;
        }

        [Test]
        public void IsRootReturnsExpectedValue()
        {
            TreeNode<int> root = this.ExampleTreeRoot();
            Assert.IsTrue(root.IsRoot);

            Assert.IsFalse(root.Children[0].IsRoot);
            Assert.IsFalse(root.Children[1].IsRoot);
            Assert.IsFalse(root.Children[0].Children[0].IsRoot);
        }

        [Test]
        public void IsLeafReturnsExpectedValue()
        {
            TreeNode<int> root = this.ExampleTreeRoot();
            Assert.IsFalse(root.IsLeaf);

            Assert.IsFalse(root.Children[0].IsLeaf);
            Assert.IsFalse(root.Children[1].IsLeaf);
            Assert.IsTrue(root.Children[0].Children[0].IsLeaf);
        }

        [Test]
        public void DepthReturnsExpectedValue()
        {
            TreeNode<int> root = this.ExampleTreeRoot();
            Assert.AreEqual(0, root.Depth);
            Assert.AreEqual(1, root.Children[0].Depth);
            Assert.AreEqual(1, root.Children[1].Depth);

            Assert.AreEqual(2, root.Children[0].Children[1].Depth);
            Assert.AreEqual(3, root.Children[1].Children[0].Children[0].Depth);
        }

        [Test]
        public void HeightReturnsExpectedValue()
        {
            TreeNode<int> root = this.ExampleTreeRoot();
            Assert.AreEqual(3, root.Height);
            Assert.AreEqual(1, root.Children[0].Height);
            Assert.AreEqual(2, root.Children[1].Height);

            Assert.AreEqual(0, root.Children[0].Children[1].Height);

            Assert.AreEqual(1, root.Children[1].Children[0].Height);
            Assert.AreEqual(0, root.Children[1].Children[0].Children[0].Height);
        }

        [Test]
        public void SiblingsReturnsExpectedValues()
        {
            TreeNode<int> root = this.ExampleTreeRoot();
            Assert.AreEqual(0, root.Siblings.Count());

            Assert.AreEqual(3, root.Children[0].Value);
            Assert.AreEqual(1, root.Children[0].Siblings.Count());
            Assert.AreEqual(4, root.Children[0].Siblings[0].Value);

            Assert.AreEqual(2, root.Children[0].Children[0].Value);
            Assert.AreEqual(2, root.Children[0].Children[0].Siblings.Count);
            Assert.AreEqual(5, root.Children[0].Children[0].Siblings[0].Value);
            Assert.AreEqual(12, root.Children[0].Children[0].Siblings[1].Value);
        }

        [Test]
        public void NodeCountReturnsCorrectCount()
        {
            TreeNode<int> root = new TreeNode<int>(5);
            Assert.AreEqual(1, root.NodeCount);

            root.AddChild(7);
            Assert.AreEqual(2, root.NodeCount);
            root.Children[0].AddChild(8);
            Assert.AreEqual(3, root.NodeCount);
            root.AddChild(6);
            Assert.AreEqual(4, root.NodeCount);
        }
    }
}
