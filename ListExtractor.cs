using HtmlAgilityPack;
using System;
using System.Net;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        // Prompt the user to enter the URL
        Console.WriteLine("Enter the URL:");
        string url = Console.ReadLine();

        // Load HTML content from the specified URL
        string htmlContent = LoadHtmlFromUrl(url);

        if (!string.IsNullOrEmpty(htmlContent))
        {
            // Load the HTML document
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);

            // Remove script elements
            foreach (var scriptNode in htmlDocument.DocumentNode.Descendants("script").ToArray())
            {
                scriptNode.Remove();
            }

            // Remove style elements
            foreach (var styleNode in htmlDocument.DocumentNode.Descendants("style").ToArray())
            {
                styleNode.Remove();
            }

            // Remove hyperlink elements
            foreach (var aNode in htmlDocument.DocumentNode.Descendants("a").ToArray())
            {
                aNode.Remove();
            }

            // Remove newline characters
            string cleanedHtml = Regex.Replace(htmlDocument.DocumentNode.InnerText, @"\r\n?|\n|\t", " ");

            // Remove navbar if it has specific class name or id
            var navbars = htmlDocument.DocumentNode.SelectNodes("//*[@class='navbar'] | //*[@id='navbar']");
            if (navbars != null)
            {
                foreach (var navbar in navbars)
                {
                    navbar.Remove();
                }
            }

            // Select the table element
            HtmlNode tableNode = htmlDocument.DocumentNode.SelectSingleNode("//table");

            if (tableNode != null)
            {
                // Extract table content
                string tableContent = tableNode.InnerText;
                Console.WriteLine("Table Content:");
                Console.WriteLine(tableContent);
            }
            else
            {
                Console.WriteLine("No table found in the HTML content.");
            }

            // Select list item elements
            var listItems = htmlDocument.DocumentNode.SelectNodes("//li");

            if (listItems != null && listItems.Count > 0)
            {
                // Extract list item content
                Console.WriteLine("\nList Item Content:");
                foreach (var listItem in listItems)
                {
                    Console.WriteLine(listItem.InnerText);
                }
            }
            else
            {
                Console.WriteLine("No list items found in the HTML content.");
            }

            // Output the cleaned HTML content
            Console.WriteLine("\nCleaned Text Content:");
            Console.WriteLine(cleanedHtml);
        }
        else
        {
            Console.WriteLine("Failed to load HTML content from the specified URL.");
        }
    }

    static string LoadHtmlFromUrl(string url)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading HTML content from URL: {ex.Message}");
            return null;
        }
    }
}
