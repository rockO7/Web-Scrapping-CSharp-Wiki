using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Generic;

class Program
    {
        static void Main(string[] args)
        {
            // Replace with the actual URL of the website you want to scrape
            var htmlUrl = "https://www.smgov.net/departments/council/content.aspx?id=13705";

            // Load the HTML content from the website
            var web = new HtmlWeb();
            var htmlDoc = web.Load(htmlUrl);

            // Extract all text content (excluding HTML tags)
            var sb = new System.Text.StringBuilder();
            foreach (var node in htmlDoc.DocumentNode.DescendantsAndSelf()) 
            {
                if (!node.HasChildNodes)
                {
                    string text = node.InnerText;
                    if (!string.IsNullOrEmpty(text))
                        sb.AppendLine(text.Trim());
                }
            }

            // Print the extracted text
            Console.WriteLine(sb.ToString());
        }
    }
