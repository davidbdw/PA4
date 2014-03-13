using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
{
    public class TrieNode
    {
        public char letter;
        public bool isWord;
        public Dictionary<char, TrieNode> children;

        public TrieNode() : this(char.MinValue, false) { }


        public TrieNode(char letter, bool isWord)
        {
            this.letter = letter;
            this.isWord = isWord;
            this.children = new Dictionary<char, TrieNode>();
        }

        public char getLetter
        {
            get { return this.letter; }
        }

        public bool ifIsWord
        {
            get { return this.isWord; }
        }

        public Dictionary<char, TrieNode> childDict
        {
            get { return this.children; }
            set { this.children = value; }
        }

        public TrieNode getChildNode(char character)
        {
            if (childDict != null)
            {
                if (childDict.ContainsKey(character))
                {
                    return childDict[character];
                }
            }
            return null;
        }

    }
}