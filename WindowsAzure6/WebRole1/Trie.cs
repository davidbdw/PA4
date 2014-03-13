using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
{
    public class Trie
    {
        public TrieNode overallRoot;
        int count = 0;

        public Trie()
        {
            this.overallRoot = new TrieNode();
        }

        public void insert(string s)
        {
            TrieNode node = this.overallRoot;

            char[] sCharArray = s.ToCharArray();
            for (int i = 0; i < sCharArray.Length; i++)
            {
                if (node.childDict.ContainsKey(sCharArray[i]))
                {
                    node = node.children[sCharArray[i]];
                }
                else
                {
                    TrieNode childNode = new TrieNode();
                    childNode.letter = sCharArray[i];
                    node.children.Add(sCharArray[i], childNode);
                    node = childNode;
                }
            }
            node.isWord = true;
        }
        //char[] sCharArray = s.ToCharArray();
        //for (int i = 0; i < sCharArray.Length; i++)
        //{
        //    if (node.childDict.ContainsKey(sCharArray[i]))
        //    {
        //        node = node.children[sCharArray[i]];
        //    }
        //    else
        //    {
        //        TrieNode childNode = new TrieNode();
        //        childNode.letter = sCharArray[i];
        //        node.children.Add(sCharArray[i], childNode);
        //        node = childNode;
        //    }                
        //}
        //node.isWord = true;


        public List<string> search(string s)
        {
            List<string> topTenResults = new List<string>();
            //topTenResults = searchHelper(this.overallRoot, s, topTenResults);
            //return topTenResults;

            TrieNode node = searchForNode(this.overallRoot, s);
            topTenResults = searchForResults(node, topTenResults, s);
            return topTenResults;

        }


        private TrieNode searchForNode(TrieNode root, string s)
        {
            TrieNode node = root;

            for (int i = 0; i < s.Length; i++)
            {
                char firstLetter = s[i];
                if (node.children.ContainsKey(firstLetter))
                {
                    node = node.children[firstLetter];
                }
            }
            return node;
        }


        private List<string> searchForResults(TrieNode start, List<string> topTenResults, string s)
        {
            TrieNode node = start;

            if (topTenResults.Count < 10)
            {
                if (node.ifIsWord)
                {
                    topTenResults.Add(s);
                    //searchForResults(node.children[, topTenResults, s);
                }
                //else
                //{
                //    node = start.children[s[0]];
                //    topTenResults = searchForResults(node, topTenResults, s);

                foreach (char letter in node.children.Keys)
                {
                    searchForResults(node.children[letter], topTenResults, s + letter);
                }

            }
            return topTenResults;
        }





        //    private bool isLegal(string s)
        //    {
        //        if (s.Split('_').Length > 2)
        //        {
        //            return false;
        //        }

        //        foreach (char letter in s)
        //        {
        //            if ((letter != ' ') && (letter != '_') && ((letter < 'a') || (letter > 'z')) && ((letter < 'A') || (letter > 'Z')))
        //            {
        //                return false;
        //            }
        //        }
        //        return true;
        //    }


        //    private List<string> searchHelper(TrieNode root, string s, List<string> topTenResults)
        //    {
        //        TrieNode node = root;
        //        //char firstLetter = Convert.ToChar(s.Substring(0, 1));

        //        for (int i = 0; i < s.Length; i++)
        //        {
        //            char firstLetter = s[0];
        //            if (node.children.ContainsKey(firstLetter))
        //            {
        //                node = node.children[firstLetter];
        //            }
        //        }

        //        return topTenResults;
        //    }
        //}

    }
}
