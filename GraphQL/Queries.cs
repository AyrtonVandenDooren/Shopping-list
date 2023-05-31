namespace Shops.GraphQl.Querries;

public class Querries
{
    public async Task<List<Shop>> GetShops([Service] IShopService shopService) => await shopService.GetShops();
    public async Task<List<Category>> GetCategories([Service] IShopService shopService) => await shopService.GetCategories();
    public async Task<List<Product>> GetProducts([Service] IShopService shopService) => await shopService.GetProducts();

    public async Task<List<Product>> GetProductsByCategoryId([Service] IShopService shopService, string CategoryId) => await shopService.GetProductsByCategoryId(CategoryId);

}