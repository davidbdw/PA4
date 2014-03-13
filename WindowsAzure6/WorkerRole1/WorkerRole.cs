using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;
using System.IO;
using System.Text;


namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        public static List<string> urls = new List<string>();
        public static List<string> robots = new List<string>();
        public static HashSet<string> visitedurls = new HashSet<string>();

        public class PageAttributes : TableEntity
        {
            public string title { get; set; }
            public string url { get; set; }
            public string date { get; set; }
            public string error { get; set; }

            public PageAttributes(string title, string url, string date, string error)
            {
                this.PartitionKey = "url";
                this.RowKey = "rowkey" + title;
                this.title = title;
                this.url = url;
                this.date = date;
                this.error = error;
            }
            public PageAttributes() { }
        }

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("WorkerRole1 entry point called", "Information");

            while (true)
            {
                Thread.Sleep(500);
                Trace.TraceInformation("Working", "Information");

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));

                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue queue = queueClient.GetQueueReference("myurls");
                queue.CreateIfNotExists();

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the table if it doesn't exist.
                CloudTable table = tableClient.GetTableReference("people");
                table.CreateIfNotExists();

                CloudQueueMessage test = new CloudQueueMessage("Hello world");
                queue.AddMessage(test);

                getRobotsTxt();

                string results = getPageHTML("http://www.cnn.com");

                getHrefLinks(results, robots);

                getXML("http://www.cnn.com/robots.txt");

                CloudQueueMessage message;
                foreach (string eachURL in urls)
                {
                    message = new CloudQueueMessage(eachURL);
                    queue.AddMessage(message);
                }

                PageAttributes pA = new PageAttributes();

            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public void getRobotsTxt()
        {
            string urlAddress = "http://www.cnn.com/robots.txt";
            string data = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.ConnectionLimit = 1;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            using (StringReader reader = new StringReader(data))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("Disallow:"))
                    {
                        if (robots.Count < 28)
                        {
                            robots.Add(line.Substring(10));
                        }
                    }
                }
            }
        }

        public static string getPageHTML(string url)
        {
            string urlAddress = url;
            string data = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.ConnectionLimit = 1;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
            return data;
        }

        public void getXML(string urlAddress)
        {
            string data = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.ConnectionLimit = 1;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }


            using (StringReader reader = new StringReader(data))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("Sitemap:"))
                    {
                        getXML(line.Substring(9));
                    }
                    if (line.Contains("http") && line.Contains(".xml"))
                    {

                        int startIndex = line.IndexOf("http");

                        string temp = line.Substring(startIndex);
                        int endIndex = temp.IndexOf(".xml");

                        getXML(line.Substring(startIndex, endIndex + 4));
                    }

                    if (line.Contains("http") && line.Contains(".html"))
                    {
                        int startIndex = line.IndexOf("http");
                        string temp = line.Substring(startIndex);

                        int endIndex = temp.IndexOf(".html");
                        urls.Add(line.Substring(startIndex, endIndex + 5));
                    }
                }
            }

        }

        public void getHrefLinks(string results, List<string> robots)
        {
            List<string> filtered = new List<string>();

            Match m;
            string HRefPattern = "href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";
            try
            {
                m = Regex.Match(results, HRefPattern,
                                RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                TimeSpan.FromSeconds(1));
                while (m.Success)
                {
                    Console.WriteLine("Found href " + m.Groups[1] + " at "
                       + m.Groups[1].Index);

                    bool alreadyVisited = false;
                    bool hasDisallow = false;
                    string newURL = m.Groups[1].Value;
                    foreach (string eachDisallow in robots)
                    {
                        if (newURL.Contains(eachDisallow))
                        {
                            hasDisallow = true;
                            break;
                        }
                    }

                    foreach (string visited in visitedurls)
                    {
                        if (newURL.Contains(visited))
                        {
                            alreadyVisited = true;
                        }
                    }

                    if (!hasDisallow)
                    {
                        if (!alreadyVisited)
                        {
                            urls.Add(newURL);
                            visitedurls.Add(newURL);
                        }

                    }
                    m = m.NextMatch();
                }
            }
            catch (RegexMatchTimeoutException)
            {
                Console.WriteLine("The matching operation timed out.");
            }
        }

    }
}
