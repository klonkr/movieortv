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
using CommandLineParser;

namespace movieortv
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineParser.CommandLineParser parser = new CommandLineParser.CommandLineParser();
            Arguments arguments = new Arguments();

            try
            {
                parser.ExtractArgumentAttributes(arguments);
                parser.ParseCommandLine(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                parser.ShowUsage();
                Environment.Exit(1);
            }

            string name = arguments.input;
            string category = arguments.category;
            string rootPath = arguments.rootPath;
            string savePath = arguments.savePath;

            string pattern = @".+?(?=.(S[0-9]|[0-9]))";
            Regex reg = new Regex(pattern);
            string searchTerm = reg.Match(name).ToString().Replace(".", " ");


            RunAsync(searchTerm, name, savePath, rootPath).Wait();

            System.IO.File.WriteAllText(@"C:\Users\anton\Documents\output.txt", $"Torrent name = {name} \n Category = {category} \n Content path \n Root path = {rootPath} \n Save path = {savePath} \n Number of files =  \n Current tracker =  \n Info  hash = ");

        }

        static async Task RunAsync(string searchTerm, string name, string savePath, string rootPath)
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
                        Directory.CreateDirectory($"{savePath}unsorted");
                        string destinationPath = $"{savePath}/unsorted/{name}";
                        CopyDirectory(rootPath, destinationPath);
                    }

                    else
                    {
                        string something = (string)token.SelectToken("Type");
                        string destinationPath = $"{savePath}/{something}/{name}";
                        Directory.CreateDirectory(destinationPath);
                        CopyDirectory(rootPath, destinationPath);
                    }
                }
            }
        }

        private static void CopyDirectory(string rootPath, string destinationPath)
        {
            foreach (string dirPath in Directory.GetDirectories(rootPath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(rootPath, destinationPath));

            foreach (string newPath in Directory.GetFiles(rootPath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(rootPath, destinationPath), true);
        }
    }
}
