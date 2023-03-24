// See https://aka.ms/new-console-template for more information
using HtmlAgilityPack;
using System.Xml;

Console.WriteLine("Extract plain text from HTML.");



static string ExtractText(string html)
{
    if (html == null)
    {
        throw new ArgumentNullException(nameof(html));
    }

    HtmlDocument doc = new();
    doc.LoadHtml(html);

    var chunks = new List<string>();

    foreach (var item in doc.DocumentNode.DescendantsAndSelf())
    {
        if (item.NodeType == HtmlNodeType.Text)
        {
            if (item.InnerText.Trim() != "")
            {
                chunks.Add(item.InnerText.Trim());
            }
        }
    }
    return String.Join(" ", chunks);
}