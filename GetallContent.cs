using HtmlAgilityPack;
using System;
using System.Text;
using System.Web; // Add this namespace for HttpUtility

class Program
{
    static void Main(string[] args)
    {
        // Replace with the actual URL of the website you want to scrape
        var htmlUrl = "https://datausa.io/profile/geo/seattle-wa/";

        // Load the HTML content from the website
        var web = new HtmlWeb();
        var htmlDoc = web.Load(htmlUrl);

        // Remove unwanted script and style elements
        htmlDoc.DocumentNode.Descendants()
            .Where(n => n.Name == "script" || n.Name == "style")
            .ToList()
            .ForEach(n => n.Remove());

        // Extract all text content (excluding HTML tags, CSS, and JavaScript)
        var sb = new StringBuilder();
        foreach (var node in htmlDoc.DocumentNode.DescendantsAndSelf())
        {
            if (!node.HasChildNodes && node.NodeType == HtmlNodeType.Text)
            {
                string text = node.InnerText;
                if (!string.IsNullOrEmpty(text))
                {
                    // Decode HTML entities
                    text = HttpUtility.HtmlDecode(text);
                    sb.AppendLine(text.Trim());
                }
            }
        }

        // Print the extracted text
        Console.WriteLine(sb.ToString());
    }
}
