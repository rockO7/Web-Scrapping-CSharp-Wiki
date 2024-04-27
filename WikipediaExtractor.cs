using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Generic;

class Program
{
    static async Task Main(string[] args)
    {
        var url = "https://en.wikipedia.org/wiki/Seattle"; // Replace with the URL you want to extract data from
        var html = await FetchHtmlContent(url);

        var importantHeadings = new List<string>
        {
            "Utilities"
        };

        foreach (var heading in importantHeadings)
        {
            var sectionContent = ExtractSectionContent(html, heading, maxParagraphs: 2, maxCharacters:  1500);
            if (sectionContent.Count > 0)
            {
                Console.WriteLine($"### {HttpUtility.HtmlDecode(heading)}");
                foreach (var content in sectionContent)
                {
                    Console.WriteLine(content);
                }
            }
            else
            {
                Console.WriteLine($"No content found for section '{heading}'.");
            }
        }
    }

    static async Task<string> FetchHtmlContent(string url)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Error fetching content from {url}: {e.Message}");
            return null;
        }
    }

    static List<string> ExtractSectionContent(string htmlContent, string sectionName, int maxParagraphs, int maxCharacters)
    {
        var sectionContent = new List<string>();
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        var sectionHeading = htmlDoc.DocumentNode.SelectSingleNode($"//span[@class='mw-headline'][text()='{sectionName}']");
        if (sectionHeading != null)
        {
            var currentParagraph = sectionHeading.ParentNode.SelectSingleNode("following-sibling::p");
            var paragraphCount = 0;

            while (currentParagraph != null && paragraphCount < maxParagraphs)
            {
                var limitedContent = LimitString(currentParagraph.InnerText.Trim(), maxCharacters);
                sectionContent.Add(HttpUtility.HtmlDecode(limitedContent));
                currentParagraph = currentParagraph.SelectSingleNode("following-sibling::p");
                paragraphCount++;
            }

            var table = sectionHeading.ParentNode.SelectSingleNode("following-sibling::table");
            if (table != null)
            {
                var rows = table.SelectNodes(".//tr");
                if (rows != null)
                {
                    foreach (var row in rows)
                    {
                        var cells = row.SelectNodes(".//td");
                        if (cells != null)
                        {
                            var rowContent = string.Join("\t", cells.Select(cell => LimitString(cell.InnerText.Trim(), 50)));
                            sectionContent.Add(rowContent);
                        }
                    }
                }
            }
        }

        return sectionContent;
    }

    static string LimitString(string text, int maxLength)
    {
        if (text.Length <= maxLength)
            return text;
        else
            return text.Substring(0, maxLength - 3) + "...";
    }
}
