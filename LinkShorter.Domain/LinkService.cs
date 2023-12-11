using Database.Mongo;
using Database.Mongo.Models;
using LinkShorter.Domain.Models;
using NUlid;

namespace LinkShorter.Domain;

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
        
        await _mongoDbService.AddTransition(shorterLink.Id);
        
        return MapToDto(shorterLink);
    }

    private static Link MapToDto(ShorterLink shorterLink)
    {
        return new Link(shorterLink.Key, shorterLink.MainLink, shorterLink.TransitionsCount);
    }

    private static string GenerateRandomCode()
    {
        const string characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var codeLength = 5;

        char[] codeArray = new char[codeLength];

        for (int i = 0; i < codeLength; i++)
            codeArray[i] = characters[Random.Shared.Next(characters.Length)];

        return new string(codeArray);
    }
}