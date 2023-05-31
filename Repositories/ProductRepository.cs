namespace Shops.Repositories;

public interface IProductRepository
{
    Task<Product> AddProduct(Product newProduct);
    Task DeleteProduct(string id);
    Task<List<Product>> GetAllProducts();
    Task<Product> GetProduct(string id);
    Task<List<Product>> GetProductsByCategoryId(string categoryId);
    Task<Product> UpdateProduct(string productId, Product product);
    Task<Product> UpdateStock(string productId, int stock);
}

public class ProductRepository : IProductRepository
{
    private readonly IMongoContext _context;

    public ProductRepository(IMongoContext context)
    {
        _context = context;
    }

    #region add
    public async Task<Product> AddProduct(Product newProduct)
    {
        await _context.ProductsCollection.InsertOneAsync(newProduct);
        return newProduct;
    }
    #endregion

    #region get all
    public async Task<List<Product>> GetAllProducts() => await _context.ProductsCollection.Find(_ => true).ToListAsync();

    #endregion

    #region get by id
    public async Task<Product> GetProduct(string id) => await _context.ProductsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

    #endregion

    //Maak een lijst van alle producten 
    #region get all products by category id
    public async Task<List<Product>> GetProductsByCategoryId(string categoryId) => await _context.ProductsCollection.Find(c => c.category.Id == categoryId).ToListAsync();
    #endregion

    #region delete
    public async Task DeleteProduct(string id)
    {
        try
        {
            var filter = Builders<Product>.Filter.Eq("Id", id);
            var result = await _context.ProductsCollection.DeleteOneAsync(filter);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    #endregion

    #region update
    // public async Task<Product> UpdateProduct(Product Product)
    // {
    //     try
    //     {
    //         var filter = Builders<Product>.Filter.Eq("Id", Product.Id);
    //         var update = Builders<Product>.Update.Set("Name", Product.Name);
    //         var result = await _context.ProductsCollection.UpdateOneAsync(filter, update);
    //         return await GetProduct(Product.Id);
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex);
    //         throw;
    //     }
    // }

    public async Task<Product> UpdateProduct(string productId, Product product)
    {
        try
        {
            var filter = Builders<Product>.Filter.Eq(e => e.Id, productId);
            var update = Builders<Product>.Update
                .Set(e => e.Name, product.Name)
                .Set(e => e.Stock, product.Stock);
            await _context.ProductsCollection.UpdateOneAsync(filter, update);
            return await GetProduct(productId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<Product> UpdateStock(string productId, int stock)
    {

        try
        {
            var filter = Builders<Product>.Filter.Eq("Id", productId);
            var update = Builders<Product>.Update.Set("Stock", stock);
            var result = await _context.ProductsCollection.UpdateOneAsync(filter, update);
            return await GetProduct(productId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    #endregion
}

