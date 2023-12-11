using System.Data.Common;
using Database.Mongo.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace Database.Mongo;

public class MongoDbService
{
    private readonly IMongoCollection<ShorterLink> _shorterlinksCollection;

    public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionUri);
        IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _shorterlinksCollection = database.GetCollection<ShorterLink>(mongoDbSettings.Value.CollectionName);
    }

    public async Task Create(ShorterLink shorterLink)
    {
        await _shorterlinksCollection.InsertOneAsync(shorterLink);
    }
    
    public async Task<ShorterLink> GetByCode(string code)
    {
        var filter = Builders<ShorterLink>.Filter.Eq("Key", code);
        return await _shorterlinksCollection.Find(filter).FirstOrDefaultAsync();
    }
    
    public async Task<ShorterLink> GetById(string id)
    {
        var filter = Builders<ShorterLink>.Filter.Eq("_id", id);
        var x =  await _shorterlinksCollection.Find(filter).FirstOrDefaultAsync();

        return x;
    }

    public async Task AddTransition(string id)
    {
        var filter = Builders<ShorterLink>.Filter.Eq(x => x.Id, id);
        
        var update = Builders<ShorterLink>.Update.Inc("TransitionsCount", 1);
        
        await _shorterlinksCollection.UpdateOneAsync(filter, update);
    }
}