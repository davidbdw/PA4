using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1
{
    public class FilterWiki
    {
        static void Main(string[] args)
        {

            string lines = "this is a test file text.";
            System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\test.txt");
            file.WriteLine(lines);
            file.Close();


        }


    }
}