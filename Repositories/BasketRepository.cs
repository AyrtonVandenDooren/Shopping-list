namespace Shops.Repositories;

public interface IBasketRepository
{
    Task<Basket> AddBasket(Basket basket);
}

public class BasketRepository : IBasketRepository
{
    private IMongoContext _context;

    public BasketRepository(IMongoContext context)
    {
        _context = context;
    }

    public async Task<Basket> AddBasket(Basket basket)
    {
        // await _context.BasketCollection.InsertOneAsync(basket);
        // return basket;
        try{
            await _context.BasketCollection.InsertOneAsync(basket);
            return basket;
        }
        catch(Exception ex){
            throw ex;
        }
    }
}