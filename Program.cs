using HtmlAgilityPack;

Console.WriteLine("Extract plain text from HTML.");

var allChunks = new List<string>();
if (args.Length > 0)
{
    string rootPath = args[0];
    TraverseDirectory(rootPath, ProcessFile);
    File.WriteAllText(rootPath+"\\allText.txt", String.Join("\r\n", allChunks));
    return 0;
}
else
    Console.WriteLine("ERROR: Please provide a folder path as argument.");
return 1;


void TraverseDirectory(string path, Action<string> action)
{
    foreach (string file in Directory.GetFiles(path))
    {
        if (file.ToLowerInvariant().EndsWith(".htm") || file.ToLowerInvariant().EndsWith("html"))
        {
            action(file);
        }
    }

    foreach (string directory in Directory.GetDirectories(path))
    {
        TraverseDirectory(directory, action);
    }
}

void ProcessFile(string filePath)
{
    string html = File.ReadAllText(filePath);
    Console.WriteLine($"Processing file: {filePath} {html.Length} characters");
    string result = ExtractText(html);
    string newFileName = filePath + ".txt";
    File.WriteAllText(newFileName, result);
    Console.WriteLine($"Plain text written to: {newFileName} {result.Length} characters");
}



string ExtractText(string html)
{
    if (html == null)
    {
        throw new ArgumentNullException(nameof(html));
    }

    HtmlDocument doc = new();
    doc.LoadHtml(html);

    var chunks = new List<string>();

    //translatable elements are h1-h6, p, a, li, tr, td, th, label, button, alt attribute, 

    foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//p|//h1|//h2|//h3|//h4|//h5|//h6|//li|//tr|//td|//th|//label|//a"))
    {
            if (node.InnerText.Trim() != "")
            {
                string toInsert = node.InnerText.Trim();
                if (!chunks.Contains(toInsert)) chunks.Add(toInsert);
                if (!allChunks.Contains(toInsert)) allChunks.Add(toInsert);
        }
    }
    return String.Join("\r\n", chunks);
}
