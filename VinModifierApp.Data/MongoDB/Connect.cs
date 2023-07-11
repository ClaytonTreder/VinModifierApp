using System;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace VinModifierApp.Data.MongoDB;

public interface IConnect
{
    MongoClient GetClient();
    IMongoDatabase GetDatabase();
    IMongoDatabase GetDatabase(string name);
}

public class Connect : IConnect
{
    private MongoClient MongoClient;
    public Connect(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB");
        var mongoClient = new MongoClient(connectionString);
        MongoClient = mongoClient;
    }

    public MongoClient GetClient()
    {
        return MongoClient;
    }

    public IMongoDatabase GetDatabase()
    {
        return MongoClient.GetDatabase("vinmodifier");
    }

    public IMongoDatabase GetDatabase(string name)
    {
        return MongoClient.GetDatabase(name);
    }

}


