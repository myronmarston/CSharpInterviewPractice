using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CSharpInterviewPractice
{
    public class TrieTreeNode
    {
        private string word;
        private int childrenCount = 0;
        private Nullable<char> character;
        public string Word { get { return word; } }
        public Nullable<char> Character { get { return character; } }
        private TrieTreeNode[] children = new TrieTreeNode[52];
        public TrieTreeNode this[char character]
        {
            get { return this.children[ArrayIndex(character)]; }
        }
        public TrieTreeNode(Nullable<char> character) : this(character, null) { }
        public TrieTreeNode(Nullable<char> character, string parentWord)
        { 
            this.character = character;
            this.word = parentWord == null ? character.ToString() : parentWord + character.ToString();
        }

        static protected int ArrayIndex(char character)
        {
            if (character >= 'A' && character <= 'Z') return character - 'A';
            else if (character >= 'a' && character <= 'z') return character - 'a';
            throw new ArgumentException("char must be between A and Z or a and z.", "char");
        }

        protected TrieTreeNode CreateChild(char childChar)
        {
            childrenCount++;
            TrieTreeNode child = new TrieTreeNode(childChar, this.Word);
            this.children[ArrayIndex(childChar)] = child;
            return child;
        }

        public void Add(string word)
        {
            Add(word.ToCharArray(), 0);
        }

        protected void Add(char[] chars, int index)
        {
            if (index >= chars.Length) return;

            char childChar = chars[index];
            TrieTreeNode child = this[childChar];
            if (child == null) child = CreateChild(childChar);

            child.Add(chars, index + 1);
        }

        public string[] Find(char[] chars, int index)
        {
            if (index >= chars.Length) return Words(); // we're at the end of the search; return all subwords...
            TrieTreeNode child = this[chars[index]];
            if (child == null) return null;
            return child.Find(chars, index + 1);
        }

        public string[] Words()
        {
            List<TrieTreeNode> kids = this.children.Where(c => c != null).ToList<TrieTreeNode>();
            if (kids.Count == 0) return new string[] { Word };

            List<string> words = new List<string>();
            foreach (TrieTreeNode kid in kids) words.AddRange(kid.Words());
            return words.ToArray<string>();
        }
    }

    class TrieTree : TrieTreeNode
    {
        public TrieTree() : base(null) { }

        public string[] Find(string word)
        {
            return Find(word.ToCharArray(), 0);
        }
    }

    [TestFixture]
    class TrieTreeTest
    {
        [Test]
        public void AddingInvalidStringThrowsException()
        {
            TrieTreeNode n = new TrieTreeNode('P');
            Assert.Throws<ArgumentException>(delegate { n.Add("@-8"); });
        }

        [Test]
        public void AddingValidStringCreatesSubNodes()
        {
            TrieTreeNode n = new TrieTreeNode('F');
            n.Add("ood");
            Assert.AreEqual('o', n['o'].Character);
            Assert.AreEqual("Fo", n['o'].Word);
            Assert.AreEqual('o', n['o']['o'].Character);
            Assert.AreEqual("Foo", n['o']['o'].Word);
            Assert.AreEqual('d', n['o']['o']['d'].Character);
            Assert.AreEqual("Food", n['o']['o']['d'].Word);
        }

        [Test]
        public void WordsWorks()
        {
            TrieTreeNode n = new TrieTreeNode('F');
            n.Add("rog");
            n.Add("ood");
            n.Add("ool");

            Assert.AreEqual(new string[] { "Food", "Fool", "Frog" }, n.Words());
            Assert.AreEqual(new string[] { "Food", "Fool" }, n['o'].Words());
            Assert.AreEqual(new string[] { "Frog" }, n['r'].Words());
            Assert.AreEqual(new string[] { "Frog" }, n['r']['o']['g'].Words());
        }

        [Test]
        public void FindReturnsMatchingStrings()
        {
            TrieTree tree = new TrieTree();
            tree.Add("Pizza");
            tree.Add("Pie");
            tree.Add("Computer");
            tree.Add("Pancake");
            
            Assert.AreEqual(new string[] { "Pancake", "Pie", "Pizza" }, tree.Find("P"));
            Assert.AreEqual(new string[] { "Pie", "Pizza" }, tree.Find("Pi"));
            Assert.AreEqual(new string[] { "Pie" }, tree.Find("Pie"));
            Assert.IsNull(tree.Find("Piezo"));
        }
    }
}
