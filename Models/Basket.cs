namespace Shops.Models;

public class Basket
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? BasketId { get; set; }
    public string? ProductId { get; set; }
    public int NumberOfItems { get; set; }
}