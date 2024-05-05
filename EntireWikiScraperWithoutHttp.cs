using HtmlAgilityPack;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var url = "https://en.wikipedia.org/wiki/Seattle_City_Council"; // Replace with the URL you want to extract data from
        var htmlWeb = new HtmlWeb();
        var htmlDoc = await htmlWeb.LoadFromWebAsync(url);

        // Example: Extract content based on specific section headings
        var sectionHeadings = htmlDoc.DocumentNode.SelectNodes("//span[@class='mw-headline']");
        if (sectionHeadings != null)
        {
            foreach (var heading in sectionHeadings)
            {
                var sectionTitle = HtmlEntity.DeEntitize(heading.InnerText.Trim()); // Decode HTML entities
                Console.WriteLine($"\n\n### {sectionTitle}\n");

                // Find the corresponding content for each section
                var sectionContent = heading.ParentNode.SelectSingleNode("following-sibling::p");
                while (sectionContent != null)
                {
                    Console.WriteLine(HtmlEntity.DeEntitize(sectionContent.InnerText.Trim())); // Decode HTML entities
                    sectionContent = sectionContent.SelectSingleNode("following-sibling::p"); // Move to the next <p> tag
                }

                // Extract table data (if available)
                var table = heading.ParentNode.SelectSingleNode("following-sibling::table");
                if (table != null)
                {
                    // Process each row in the table
                    var rows = table.SelectNodes(".//tr");
                    if (rows != null)
                    {
                        foreach (var row in rows)
                        {
                            var cells = row.SelectNodes(".//td");
                            if (cells != null)
                            {
                                foreach (var cell in cells)
                                {
                                    Console.Write(HtmlEntity.DeEntitize(cell.InnerText.Trim()) + "\t");
                                }
                                Console.WriteLine(); // Newline after each row
                            }
                        }
                    }
                }
            }
        }
    }
}
