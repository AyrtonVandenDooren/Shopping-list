namespace Shops.ShopServices;

public interface IShopService
{
    Task<Basket> AddBasket(Basket newBasket);
    Task<Category> AddCategory(Category newCategory);
    Task<Product> AddProduct(Product newProduct);
    Task<Shop> AddShop(Shop newShop);
    Task DeleteShop(string shopId);
    Task<List<Category>> GetCategories();
    Task<Category> GetCategory(string id);
    Task<List<Category>> GetCategoryByShopId(string shopId);
    Task<Product> GetProduct(string id);
    Task<List<Product>> GetProducts();
    Task<List<Product>> GetProductsByCategoryId(string categoryId);
    Task<Shop> GetShopById(string id);
    Task<List<Shop>> GetShops();
    Task SetupDummyData();
    Task<Product> UpdateProduct(string productId, Product product);
}

public class ShopService : IShopService
{
    public readonly ICategoryRepository _categoryRepository;
    public readonly IShopRepository _shopRepository;
    public readonly IProductRepository _productRepository;
    private readonly IBasketRepository _basketRepository;


    public ShopService(IShopRepository shopRepository, ICategoryRepository categoryRepository, IProductRepository productRepository, IBasketRepository basketRepository)
    {
        _shopRepository = shopRepository;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _basketRepository = basketRepository;
    }

    #region Toevoegen

    public async Task<Shop> AddShop(Shop newShop)
    {
        await _shopRepository.AddShop(newShop);
        return newShop;
    }
    public async Task<Category> AddCategory(Category newCategory)
    {
        await _categoryRepository.AddCategory(newCategory);
        return newCategory;
    }

    public async Task<Product> AddProduct(Product newProduct)
    {
        await _productRepository.AddProduct(newProduct);
        return newProduct;
    }

    public async Task<Basket> AddBasket(Basket newBasket)
    {
        if (newBasket == null) throw new ArgumentException();

        Product product = await _productRepository.GetProduct(newBasket.ProductId);
        if (product == null) throw new ArgumentException();

        var basket = await _basketRepository.AddBasket(newBasket);

        await _productRepository.UpdateStock(basket.ProductId, ++product.Stock);

        return basket;
    }

    #endregion

    #region Get all

    public async Task<List<Shop>> GetShops() => await _shopRepository.GetAllShops();
    public async Task<List<Category>> GetCategories() => await _categoryRepository.GetAllCategorys();
    public async Task<List<Product>> GetProducts() => await _productRepository.GetAllProducts();


    #endregion

    #region Get by id

    public async Task<List<Category>> GetCategoryByShopId(string shopId) => await _categoryRepository.GetCategoryByShopId(shopId);

    public async Task<List<Product>> GetProductsByCategoryId(string categoryId) => await _productRepository.GetProductsByCategoryId(categoryId);

    public async Task<Shop> GetShopById(string id) => await _shopRepository.GetShop(id);

    public async Task<Category> GetCategory(string id) => await _categoryRepository.GetCategory(id);

    public async Task<Product> GetProduct(string id) => await _productRepository.GetProduct(id);


    #endregion

    #region DummyData
    public async Task SetupDummyData()
    {
        if (true == (await _shopRepository.GetAllShops()).Any())
        {
            var shops = new List<Shop>()
            {
                new Shop()
                {
                    Name = "Test"
                },
                };

            foreach (var shop in shops)
                await _shopRepository.AddShop(shop);
        }
        if (!(await _shopRepository.GetAllShops()).Any())

        {
            var shops = new List<Shop>()
            {
                new Shop()
                {
                    Name = "Test"
                },
                new Shop()
                {
                    Name = "Test1"
                },
                new Shop()
                {
                    Name = "Test3"
                },
                new Shop()
                {
                    Name = "Test4"
                }
            };

            foreach (var shop in shops)
                await _shopRepository.AddShop(shop);
        }

        if (!(await _categoryRepository.GetAllCategorys()).Any())
        {
            var shops = await _shopRepository.GetAllShops();
            var categorys = new List<Category>()
            {
                new Category(){

                    Name = "vlees",
                    shop = shops[0],

                },
                new Category(){

                    Name = "vis",
                    shop = shops[0],
                },
                new Category(){

                    Name = "vlees",
                    shop = shops[1],
                },
                new Category(){

                    Name = "vlees",
                    shop = shops[2],
                },
                new Category(){

                    Name = "vlees",
                    shop = shops[3],
                }
            };
            foreach (var category in categorys)
                await _categoryRepository.AddCategory(category);
        }

        if (!(await _productRepository.GetAllProducts()).Any())
        {
            var categorys = await _categoryRepository.GetAllCategorys();
            var products = new List<Product>()
        {
            new Product(){

                Name = "kaas",
                category = categorys[0]
            },
            new Product(){

                Name = "kaas",
                category = categorys[1]
            },
            new Product(){

                Name = "kaas",
                category = categorys[2]
            },
            new Product(){

                Name = "kaas",
                category = categorys[0]
            },
            new Product(){

                Name = "kaas",
                category = categorys[0]
            }
        };
            foreach (var product in products)
                await _productRepository.AddProduct(product);
        }
    }

    #endregion

    #region Update
    public async Task<Product> UpdateProduct(string productId, Product product) => await _productRepository.UpdateProduct(productId, product);



    #endregion

    #region Delete
    public async Task DeleteShop(string shopId) => await _shopRepository.DeleteShop(shopId);

    #endregion
}