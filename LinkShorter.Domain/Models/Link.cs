namespace LinkShorter.Domain.Models;

public class Link
{
    private string Key { get; }
    public string MainLink { get; }
    public int TransitionsCount { get; set; }
    
    public string ShortLink => $"https://localhost:7148/Link/{Key}";

    public Link(string key, string mainLink, int transitionsCount)
    {
        Key = key;
        MainLink = mainLink;
        TransitionsCount = transitionsCount;
    }
}