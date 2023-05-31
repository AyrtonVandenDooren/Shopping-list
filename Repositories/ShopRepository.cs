namespace Shops.Repositories;

public interface IShopRepository
{
    Task<Shop> AddShop(Shop newShop);
    Task DeleteShop(string id);
    Task<List<Shop>> GetAllShops();
    Task<Shop> GetShop(string id);
    Task<Shop> UpdateShop(Shop Shop);
}

public class ShopRepository : IShopRepository
{
    private readonly IMongoContext _context;

    public ShopRepository(IMongoContext context)
    {
        _context = context;
    }

    #region add
    public async Task<Shop> AddShop(Shop newShop)
    {
        await _context.ShopsCollection.InsertOneAsync(newShop);
        return newShop;
    }

    #endregion

    #region get all
    public async Task<List<Shop>> GetAllShops() => await _context.ShopsCollection.Find(_ => true).ToListAsync();

    #endregion

    #region get by id
    public async Task<Shop> GetShop(string id) => await _context.ShopsCollection.Find(s => s.Id == id).FirstOrDefaultAsync();



    #endregion

    #region delete
    public async Task DeleteShop(string id) => await _context.ShopsCollection.DeleteOneAsync(s => s.Id == id);

    #endregion

    #region update
    public async Task<Shop> UpdateShop(Shop Shop)
    {
        try
        {
            var filter = Builders<Shop>.Filter.Eq("Id", Shop.Id);
            var update = Builders<Shop>.Update.Set("Name", Shop.Name);
            var result = await _context.ShopsCollection.UpdateOneAsync(filter, update);
            return await GetShop(Shop.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    #endregion
}


