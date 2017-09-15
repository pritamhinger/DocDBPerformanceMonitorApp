using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DocDBPerformanceMonitorApp
{
    class Program
    {

        static void Main(string[] args)
        {
            Task.WaitAll(InitiateMonitoring());
        }

        public static async Task InitiateMonitoring()
        {
            string RequestURL = "http://wsreportingondocdb.azurewebsites.net/api/log/{0}";
            string[] customers = { "Google", "Apple", "Microsoft" };
            foreach (var customer in customers)
            {
                string json = await GetRecordsForCustomer(string.Format(RequestURL, customer));
                object obj = JsonConvert.DeserializeObject(json);
                Console.WriteLine(json);
            }
        }

        public static async Task<string> GetRecordsForCustomer( string url)
        {
            var accept = "application/json";
            
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Accept = accept;
            
            Task<WebResponse> getResponseTask = req.GetResponseAsync();
            
            WebResponse response = await getResponseTask;
            Console.WriteLine("Content length is {0}", response.ContentLength);
            Console.WriteLine("Content type is {0}", response.ContentType);
            
            Stream stream = response.GetResponseStream();

            StreamReader readStream = new StreamReader(stream, Encoding.UTF8);

            var content = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            return content;
        }
    }
}
