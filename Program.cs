using HtmlAgilityPack;


Console.WriteLine("Extract plain text from HTML.");
if (args.Length > 0)
{
    string rootPath = args[0];
    TraverseDirectory(rootPath, ProcessFile);
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
