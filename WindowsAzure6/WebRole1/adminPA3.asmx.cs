using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Configuration;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure;
using System.Net;
using System.IO;
using System.Web.Hosting;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Services;

namespace WebRole1
{
    /// <summary>
    /// Summary description for adminPA3
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class adminPA3 : System.Web.Services.WebService
    {
        public static List<string> urls = new List<string>();
        public string path = HostingEnvironment.ApplicationPhysicalPath + @"\robots.txt";
        public static List<string> robots = new List<string>();

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

        [WebMethod]
        public void ClearIndex()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString")); //App?

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("myurls");
            queue.Clear();

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Create the table if it doesn't exist.
            CloudTable table = tableClient.GetTableReference("people");
            table.CreateIfNotExists();

            TableOperation retrieveOperation = TableOperation.Retrieve("url", "rowkey");
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Assign the result to a CustomerEntity.
            PageAttributes deleteEntity = (PageAttributes)retrievedResult.Result;


            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                table.Execute(deleteOperation);
                Console.WriteLine("Entity deleted.");
            }
            else
            {
                Console.WriteLine("Could not retrieve the entity.");
            }

        }

        [WebMethod]
        public void StartCrawling(string url)
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]); 

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue commQueue = queueClient.GetQueueReference("command");
            commQueue.CreateIfNotExists();

            CloudQueue queue = queueClient.GetQueueReference("myurls");
            queue.CreateIfNotExists();

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist.
            CloudTable table = tableClient.GetTableReference("people");
            table.CreateIfNotExists();

            CloudQueueMessage startMessage = new CloudQueueMessage("start");
            commQueue.AddMessage(startMessage);
        }


        [WebMethod]
        public void GetPageTitle()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("people");
        }
    }
}
