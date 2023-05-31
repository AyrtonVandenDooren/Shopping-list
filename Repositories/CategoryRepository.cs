namespace Shops.Repositories;

public interface ICategoryRepository
{
    Task<Category> AddCategory(Category newCategory);
    Task DeleteCategory(string id);
    Task<List<Category>> GetAllCategorys();
    Task<Category> GetCategory(string id);
    Task<List<Category>> GetCategoryByShopId(string shopId);
    Task<Category> UpdateCategory(Category Category);
}

public class CategoryRepository : ICategoryRepository
{
    //ctor
    // CategoryRepository()
    // {
    //     //default in een lijst.
    // }
    private readonly IMongoContext _context;

    public CategoryRepository(IMongoContext context)
    {
        _context = context;
    }

    #region add
    public async Task<Category> AddCategory(Category newCategory)
    {
        await _context.CategorysCollection.InsertOneAsync(newCategory);
        return newCategory;
    }
    #endregion

    #region get all
    public async Task<List<Category>> GetAllCategorys() => await _context.CategorysCollection.Find(_ => true).ToListAsync();

    #endregion

    #region get by id
    // public async Task<Category> GetCategory(string id) => await _context.CategorysCollection.Find<Category>(id).FirstOrDefaultAsync();
    public async Task<Category> GetCategory(string Id) => await _context.CategorysCollection.Find(c => c.Id == Id).FirstOrDefaultAsync();


    // public async Task<Product> GetProductById(string productId) => await _context.ProductsCollection.Find(p => p.Id == productId).SingleOrDefaultAsync();

    #endregion
    //Krijg alle category's van een shop.
    #region get all category's by shop id
    public async Task<List<Category>> GetCategoryByShopId(string shopId) => await _context.CategorysCollection.Find(c => c.shop.Id == shopId).ToListAsync();
    #endregion

    #region delete
    public async Task DeleteCategory(string id)
    {
        try
        {
            var filter = Builders<Category>.Filter.Eq("Id", id);
            var result = await _context.CategorysCollection.DeleteOneAsync(filter);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    #endregion

    #region update
    public async Task<Category> UpdateCategory(Category Category)
    {
        try
        {
            var filter = Builders<Category>.Filter.Eq("Id", Category.Id);
            var update = Builders<Category>.Update.Set("Name", Category.Name);
            var result = await _context.CategorysCollection.UpdateOneAsync(filter, update);
            return await GetCategory(Category.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    #endregion
}


