using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using Microsoft.WindowsAzure;
using System.Web.Hosting;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace WebRole1
{
    /// <summary>
    /// Summary description for WebServicePA2
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebServicePA2 : System.Web.Services.WebService
    {
        static Trie trie = new Trie();

        string downloadedFilePath = "@Desktop:\\azureBlobFile.txt";

        public string filePath = HostingEnvironment.ApplicationPhysicalPath + @"wikiFilteredLowerCase.txt";      

        private PerformanceCounter theMemCounter = new PerformanceCounter("Memory", "Available MBytes");

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getBlobFileIntoTrie()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference("finalblob");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("wikiFilteredLowerCase.txt");

            using (StreamReader sr = new StreamReader(blockBlob.OpenRead()))
            {
                try
                {
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        trie.insert(line);
                    }
                }
                catch (Exception e) 
                {
                    Console.WriteLine(e);
                }
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void readFileIntoTrie()
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (sr.EndOfStream == false)
                {
                    string line = sr.ReadLine();
                    try
                    {
                        trie.insert(line);
                    }
                    catch (OutOfMemoryException)
                    {
                        break;
                    }
                }
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> searchInTrie(string userInput)
        {
            getBlobFileIntoTrie();
            List<string> results = trie.search(userInput);
            return results;
            //return new JavaScriptSerializer().Serialize(results);
        }

   }
}

