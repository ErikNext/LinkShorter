using System.CodeDom.Compiler;
using Database.Mongo;
using Database.Mongo.Models;
using LinkShorterApi.Models;
using NUlid;

namespace LinkShorterApi.Services;

public class LinkService
{
    private readonly MongoDbService _mongoDbService;

    public LinkService(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    public async Task<Link> Create(string link)
    {
        ShorterLink shorterLink = new ShorterLink(
            Ulid.NewUlid().ToString(),
            GenerateRandomCode(),
            link);

        await _mongoDbService.Create(shorterLink);

        return MapToDto(shorterLink);
    }
    
    public async Task<Link> Get(string id)
    {
        var shorterLink = await _mongoDbService.GetById(id);
        
        if (shorterLink == null)
        {
            throw new Exception("Can`t find this link");
        }
        
        return MapToDto(shorterLink);
    }
    
    public async Task<Link> Move(string code)
    {
        var shorterLink = await _mongoDbService.GetByCode(code);

        if (shorterLink == null)
        {
            throw new Exception("Can`t find this link");
        }
        
        await _mongoDbService.AddTransition(shorterLink);
        
        return MapToDto(shorterLink);
    }

    private static Link MapToDto(ShorterLink shorterLink)
    {
        return new Link(shorterLink.Key, shorterLink.MainLink, shorterLink.TransitionsCount);
    }

    static string GenerateRandomCode()
    {
        Random random = new Random();
        const string characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int codeLength = 5;

        char[] codeArray = new char[5];

        for (int i = 0; i < codeLength; i++)
        {
            codeArray[i] = characters[random.Next(characters.Length)];
        }

        string generatedCode = new string(codeArray);
        return generatedCode;
    }
}