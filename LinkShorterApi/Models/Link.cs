namespace LinkShorterApi.Models;

public class Link
{
    public string Key { get; set; }
    public string MainLink { get; set; }
    public int TransitionsCount { get; set; }
    public string ShortLink => $"https://localhost:7148/LinkConroller/{Key}";

    public Link(string key, string mainLink, int transitionsCount)
    {
        Key = key;
        MainLink = mainLink;
        TransitionsCount = transitionsCount;
    }
}