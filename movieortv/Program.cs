using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace movieortv
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = args[0];
            string pattern = @".+?(?=.(S[0-9]|[0-9]))";
            Regex reg = new Regex(pattern);
            string searchTerm = reg.Match(input).ToString();



            RunAsync(searchTerm).Wait();
        }

        static async Task RunAsync(string searchTerm)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.omdbapi.com/?");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"http://www.omdbapi.com/?t={searchTerm}");

                if (response.IsSuccessStatusCode)
                {
                    string stringItem = await response.Content.ReadAsStringAsync();
                    JToken token = JObject.Parse(stringItem);

                    string error = (string)token.SelectToken("Response");

                    if (error == "False")
                    {
                        Console.WriteLine("Item not found!");
                    } else
                    {
                        string something = (string)token.SelectToken("Type");
                        Directory.CreateDirectory(something);
                        IEnumerable<string> files = Directory.EnumerateFileSystemEntries("/");
                    }
                    
                }
            }
        }
    }
}
