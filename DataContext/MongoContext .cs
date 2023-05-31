namespace Shops.Context;

public interface IMongoContext
{
    IMongoClient Client { get; }
    IMongoDatabase Database { get; }
    IMongoCollection<Shop> ShopsCollection { get; }
    IMongoCollection<Category> CategorysCollection { get; }
    IMongoCollection<Product> ProductsCollection { get; }
    IMongoCollection<Basket> BasketCollection { get; }
}

public class MongoContext : IMongoContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;

    private readonly DatabaseSettings _settings;

    public IMongoClient Client
    {
        get
        {
            return _client;
        }
    }
    public IMongoDatabase Database => _database;

    public MongoContext(IOptions<DatabaseSettings> dbOptions)
    {
        _settings = dbOptions.Value;
        _client = new MongoClient(_settings.ConnectionString);
        _database = _client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoCollection<Shop> ShopsCollection
    {
        get
        {
            return _database.GetCollection<Shop>(_settings.ShopsCollection);
        }
    }

    public IMongoCollection<Category> CategorysCollection
    {
        get
        {
            return _database.GetCollection<Category>(_settings.CategorysCollection);
        }
    }
    public IMongoCollection<Product> ProductsCollection
    {
        get
        {
            return _database.GetCollection<Product>(_settings.ProductsCollection);
        }
    }

    public IMongoCollection<Basket> BasketCollection
    {
        get
        {
            return _database.GetCollection<Basket>(_settings.BasketCollection);
        }
    }



}